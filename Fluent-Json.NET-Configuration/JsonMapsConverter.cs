using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using FluentJsonNet.Utils;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentJsonNet
{
    public class JsonMapsConverter : JsonConverter
    {
        private readonly List<JsonMapBase> maps;

        public JsonMapsConverter([NotNull] IEnumerable<JsonMapBase> jsonMaps)
        {
            if (jsonMaps == null)
                throw new ArgumentNullException(nameof(jsonMaps));

            this.maps = new List<JsonMapBase>(jsonMaps);
        }

        public IReadOnlyCollection<JsonMapBase> Maps => new ReadOnlyCollection<JsonMapBase>(this.maps);

        [ThreadStatic]
        private int blockOnce;

        public override bool CanConvert(Type objectType)
        {
            if (this.blockOnce == 1)
            {
                this.blockOnce = 0;
                return false;
            }

            // the question here is: is this objectType associated with any class hierarchy in the `JsonMap` list?
            List<JsonMapBase> listOfMaps;
            List<JsonSubclassMap> listOfSubclassMaps;
            var ex = this.GetInfoFromTypeCached(objectType, out listOfMaps, out listOfSubclassMaps);
            return ex == null;
        }

        private readonly Dictionary<Type, Tuple<Exception, List<JsonMapBase>, List<JsonSubclassMap>>> dicTypeData =
                     new Dictionary<Type, Tuple<Exception, List<JsonMapBase>, List<JsonSubclassMap>>>();

        private Exception GetInfoFromTypeCached(Type objectType, out List<JsonMapBase> listOfMaps, out List<JsonSubclassMap> listOfSubclassMaps)
        {
            Tuple<Exception, List<JsonMapBase>, List<JsonSubclassMap>> data;
            lock (this.dicTypeData)
                if (!this.dicTypeData.TryGetValue(objectType, out data))
                {
                    var list = new List<JsonMapBase>();
                    var sublist = this.GetJsonSubclassMapsFromType(objectType);
                    var ex = this.GetJsonMapsFromType(objectType, list);
                    this.dicTypeData[objectType] = data = Tuple.Create(ex, list, sublist);
                }

            listOfSubclassMaps = data.Item3;
            listOfMaps = data.Item2;
            return data.Item1;
        }

        private Exception GetJsonMapsFromType(Type objectType, List<JsonMapBase> listOfMaps)
        {
            var acceptedMaps = this.maps
                .Where(x => x.AcceptsType(objectType))
                .Where(x => x.SerializedType.IsAssignableFrom(objectType))
                .WithMax(x => -TypeDistance(x.SerializedType, objectType))
                .ToList();

            if (acceptedMaps.Count == 0)
                return new Exception($"`No maps for the type `{objectType.Name}`");

            if (acceptedMaps.Count > 1)
                return new Exception($"Ambiguous maps for the type `{objectType.Name}`");

            var singleMap = acceptedMaps[0];
            listOfMaps.Add(singleMap);
            if (singleMap is JsonSubclassMap)
            {
                // need to find the base type JsonMap
                return this.GetJsonMapsFromType(objectType.BaseType, listOfMaps);
            }

            return null;
        }

        private List<JsonSubclassMap> GetJsonSubclassMapsFromType(Type objectType)
        {
            var acceptedMaps = this.maps
                .OfType<JsonSubclassMap>()
                .Where(x => objectType.IsAssignableFrom(x.SerializedType))
                .ToList();

            var listOfSubclassMaps = new List<JsonSubclassMap>(acceptedMaps);

            return listOfSubclassMaps;
        }

        private static int TypeDistance(Type baseType, Type subType)
        {
            var it = 0;
            for (; subType != baseType; it++)
            {
                if (subType == null) throw new Exception("The type does not inherit from the indicated type.");
                subType = subType.BaseType;
            }
            return it;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<JsonMapBase> listOfMaps;
            List<JsonSubclassMap> listOfSubclassMaps;
            var ex = this.GetInfoFromTypeCached(value.GetType(), out listOfMaps, out listOfSubclassMaps);
            if (ex != null)
                throw ex;

            while (this.blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            this.blockOnce = 1;
            var jo = JObject.FromObject(value);

            // finding discriminator field name
            var jsonMap = listOfMaps.OfType<JsonMap>().Single();
            var discriminatorFieldName = jsonMap.DiscriminatorFieldName;
            if (discriminatorFieldName != null)
            {
                if (jsonMap is IAndSubtypes)
                {
                    var jsonMapWithSubtypes = jsonMap as IAndSubtypes;

                    var discriminatorFieldValue = jsonMapWithSubtypes.DiscriminatorFieldValueGetter != null
                        ? jsonMapWithSubtypes.DiscriminatorFieldValueGetter(value.GetType())
                        : value.GetType().AssemblyQualifiedName;

                    jo.Add(discriminatorFieldName, discriminatorFieldValue);
                }
                else
                {
                    var discriminatorFieldValue = listOfMaps.OfType<JsonSubclassMap>().First().DiscriminatorFieldValue;
                    jo.Add(discriminatorFieldName, discriminatorFieldValue);
                }
            }

            jo.WriteTo(writer, serializer.Converters.ToArray());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // the question here is: is this objectType associated with any class hierarchy in the `JsonMap` list?
            List<JsonMapBase> listOfMaps;
            List<JsonSubclassMap> listOfSubclassMaps;
            var ex = this.GetInfoFromTypeCached(objectType, out listOfMaps, out listOfSubclassMaps);
            if (ex != null)
                throw ex;

            while (this.blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            if (reader.TokenType == JsonToken.Null)
            {
                var isNullable = Nullable.GetUnderlyingType(objectType) != null || !objectType.IsValueType;
                if (isNullable)
                    return null;
            }

            var jo = JObject.Load(reader);

            // finding discriminator field name
            var value = reader.Value;

            var jsonMap = listOfMaps.OfType<JsonMap>().Single();
            var discriminatorFieldName = jsonMap.DiscriminatorFieldName;
            if (discriminatorFieldName != null)
            {
                var discriminatorFieldValueToken = jo[discriminatorFieldName];
                if (discriminatorFieldValueToken.Type == JTokenType.String)
                {
                    var discriminatorFieldValue = discriminatorFieldValueToken.Value<string>();

                    if (jsonMap is IAndSubtypes)
                    {
                        var jsonMapWithSubtypes = jsonMap as IAndSubtypes;
                        value = jsonMapWithSubtypes.CreateObject(discriminatorFieldValue);
                        serializer.Populate(jo.CreateReader(), value);
                    }
                    else
                    {
                        var subclassMap = listOfSubclassMaps
                            .SingleOrDefault(x => x.DiscriminatorFieldValue == discriminatorFieldValue);

                        if (subclassMap != null)
                        {
                            value = subclassMap.CreateNew();
                            serializer.Populate(jo.CreateReader(), value);
                        }
                    }
                }
            }

            return value;
        }
    }
}