using FluentJsonNet.Utils;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

            this.jsonMapsTree = this.GetJsonMapsTree();
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
            var cachedData = this.GetInfoFromTypeCached(objectType);
            return cachedData != null;
        }

        private readonly Dictionary<Type, JsonMapsCache> dicTypeData = new Dictionary<Type, JsonMapsCache>();

        private readonly JsonMapsTree jsonMapsTree;

        private JsonMapsCache GetInfoFromTypeCached(Type objectType)
        {
            JsonMapsCache data;
            lock (this.dicTypeData)
                if (!this.dicTypeData.TryGetValue(objectType, out data))
                {
                    var listOfMappers = this.jsonMapsTree.GetMappers(objectType).ToArray();
                    var subMappers = this.jsonMapsTree.GetSubpathes(objectType).ToArray();
                    if (listOfMappers.Length == 0 && subMappers.Length == 0)
                        this.dicTypeData[objectType] = null;
                    else
                        this.dicTypeData[objectType] = data = new JsonMapsCache { Mappers = listOfMappers, SubMappers = subMappers };
                    return data;
                }
                else
                {
                    return data;
                }
        }

        private JsonMapsTree GetJsonMapsTree()
        {
            var dictionary = new Dictionary<Type, JsonMapsTreeNode>();

            foreach (var jsonMapBase in this.maps)
            {
                var current = jsonMapBase.SerializedType;
                JsonMapsTreeNode node;
                if (!dictionary.TryGetValue(current, out node))
                {
                    var acceptedMaps = this.maps
                        .Where(x => x.AcceptsType(current))
                        .Where(x => x.SerializedType.IsAssignableFrom(current))
                        .WithMax(x => -TypeDistance(x.SerializedType, current))
                        .ToList();

                    if (acceptedMaps.Count == 0)
                        throw new Exception($"`No maps for the type `{current.Name}`");

                    if (acceptedMaps.Count > 1)
                        throw new Exception($"Ambiguous maps for the type `{current.Name}`");

                    Type parent = null;
                    if (current.BaseType != null)
                    {
                        var acceptedParentMaps = this.maps
                            .Where(x => x.AcceptsType(current.BaseType))
                            .Where(x => x.SerializedType.IsAssignableFrom(current.BaseType))
                            .WithMax(x => -TypeDistance(x.SerializedType, current.BaseType))
                            .ToList();

                        if (acceptedParentMaps.Count > 1)
                            throw new Exception($"Ambiguous maps for the type `{current.BaseType?.Name}`");

                        if (acceptedParentMaps.Count > 0 && jsonMapBase is JsonMap)
                            throw new Exception($"Sub mappers must be subclasses of `{typeof(JsonSubclassMap).Name}`: `{jsonMapBase.GetType().Name}`");

                        if (acceptedParentMaps.Count > 0)
                            parent = acceptedParentMaps[0].SerializedType;
                    }

                    var acceptedChildMaps = this.maps
                        .Where(x => current.IsAssignableFrom(x.SerializedType))
                        .Where(x => TypeDistance(current, x.SerializedType) == 1)
                        .ToList();

                    var children = acceptedChildMaps.Select(x => x.SerializedType).ToArray();

                    if (jsonMapBase is IAndSubtypes && children.Length > 0)
                        throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                    dictionary[current] = node = new JsonMapsTreeNode(jsonMapBase, parent, children);
                }
            }

            return new JsonMapsTree(dictionary);
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
            var objectType = value?.GetType();
            var cachedData = this.GetInfoFromTypeCached(objectType);

            while (this.blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            this.blockOnce = 1;
            var jo = JObject.FromObject(value);

            // finding discriminator field names
            string discriminatorFieldValue = null;
            foreach (var jsonMapBase in cachedData.Mappers)
            {
                if (jsonMapBase is IAndSubtypes)
                {
                    if (cachedData.Mappers.Length > 1)
                        throw new Exception("Json mapper of type `IAndSubtypes` cannot be combined with other mappers.");

                    var jsonMapWithSubtypes = jsonMapBase as IAndSubtypes;

                    discriminatorFieldValue = jsonMapWithSubtypes.DiscriminatorFieldValueGetter != null
                        ? jsonMapWithSubtypes.DiscriminatorFieldValueGetter(value.GetType())
                        : value.GetType().AssemblyQualifiedName;

                    jo.Add(jsonMapBase.DiscriminatorFieldName, discriminatorFieldValue);
                    discriminatorFieldValue = null;
                }
                else
                {
                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        if (discriminatorFieldValue == null)
                            throw new Exception("Value of discriminator field not defined by subclass map.");

                        jo.Add(jsonMapBase.DiscriminatorFieldName, discriminatorFieldValue);
                        discriminatorFieldValue = null;
                    }

                    var jsonSubclassMap = jsonMapBase as JsonSubclassMap;
                    if (jsonSubclassMap?.DiscriminatorFieldValue != null)
                        discriminatorFieldValue = jsonSubclassMap.DiscriminatorFieldValue;
                }
            }

            if (discriminatorFieldValue != null)
                throw new Exception("Discriminating value not set to any field.");

            jo.WriteTo(writer, serializer.Converters.ToArray());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // the question here is: is this objectType associated with any class hierarchy in the `JsonMap` list?
            var cachedData = this.GetInfoFromTypeCached(objectType);

            while (this.blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            if (reader.TokenType == JsonToken.Null)
            {
                var isNullable = Nullable.GetUnderlyingType(objectType) != null || !objectType.IsValueType;
                if (isNullable)
                    return null;
            }

            var jo = JObject.Load(reader);

            var value = reader.Value;

            // Looking for discriminators:
            // - match the discriminators of the type given by `objectType`
            // - match the discriminators of subtypes of `objectType`

            // STEP 1: match the discriminators of the type given by `objectType`
            string discriminatorFieldValue = null;
            bool foundBaseIAndSubtypes = false;
            JsonMapBase classMapToUse = null;
            foreach (var jsonMapBase in cachedData.Mappers.Reverse())
            {
                if (foundBaseIAndSubtypes)
                    throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                if (jsonMapBase is IAndSubtypes)
                {
                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        var discriminatorFieldValueToken = jo[jsonMapBase.DiscriminatorFieldName];
                        if (discriminatorFieldValueToken?.Type == JTokenType.String)
                            discriminatorFieldValue = discriminatorFieldValueToken.Value<string>();
                    }

                    foundBaseIAndSubtypes = true;
                    var jsonMapWithSubtypes = jsonMapBase as IAndSubtypes;
                    value = jsonMapWithSubtypes.CreateObject(discriminatorFieldValue);
                    serializer.Populate(jo.CreateReader(), value);
                    discriminatorFieldValue = null;
                }
                else
                {
                    var jsonSubclassMap = jsonMapBase as JsonSubclassMap;
                    if (jsonSubclassMap?.DiscriminatorFieldValue != null && discriminatorFieldValue != null)
                    {
                        if (jsonSubclassMap.DiscriminatorFieldValue != discriminatorFieldValue)
                            throw new Exception($"Discriminator does not match.");

                        discriminatorFieldValue = null;
                    }

                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        var discriminatorFieldValueToken = jo[jsonMapBase.DiscriminatorFieldName];
                        if (discriminatorFieldValueToken?.Type == JTokenType.String)
                            discriminatorFieldValue = discriminatorFieldValueToken.Value<string>();
                    }

                    classMapToUse = jsonMapBase;
                }
            }


            // STEP 2: match the discriminators of subtypes of `objectType`
            JsonMapBase subclassMapToUse = null;
            foreach (var jsonSubmappers in cachedData.SubMappers)
            {
                string subDiscriminatorFieldValue = discriminatorFieldValue;
                bool foundSubIAndSubtypes = foundBaseIAndSubtypes;
                JsonMapBase subclassMapToUse2 = null;
                foreach (var submapper in jsonSubmappers.Reverse())
                {
                    if (foundSubIAndSubtypes)
                        throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                    if (submapper is IAndSubtypes)
                    {
                        if (submapper.DiscriminatorFieldName != null)
                        {
                            var discriminatorFieldValueToken = jo[submapper.DiscriminatorFieldName];
                            if (discriminatorFieldValueToken?.Type == JTokenType.String)
                                subDiscriminatorFieldValue = discriminatorFieldValueToken.Value<string>();

                            if (subDiscriminatorFieldValue == null)
                                throw new Exception($"Discriminator field not found.");
                        }

                        foundSubIAndSubtypes = true;
                        var jsonMapWithSubtypes = submapper as IAndSubtypes;
                        value = jsonMapWithSubtypes.CreateObject(subDiscriminatorFieldValue);
                        serializer.Populate(jo.CreateReader(), value);
                        subDiscriminatorFieldValue = null;
                    }
                    else
                    {
                        var jsonSubclassMap = submapper as JsonSubclassMap;
                        if (jsonSubclassMap != null)
                        {
                            if (jsonSubclassMap.DiscriminatorFieldValue != null)
                            {
                                if (jsonSubclassMap.DiscriminatorFieldValue != subDiscriminatorFieldValue)
                                    break;

                                if (subclassMapToUse != null)
                                    throw new Exception($"Ambiguous maps for the type `{submapper.SerializedType.Name}`");

                                subDiscriminatorFieldValue = null;
                                subclassMapToUse2 = jsonSubclassMap;
                            }
                        }
                        else
                        {
                            if (cachedData.Mappers.Length > 0)
                                throw new Exception($"Sub mappers must be subclasses of `{typeof(JsonSubclassMap).Name}`: `{cachedData.Mappers[0].GetType().Name}`");

                            if (subclassMapToUse != null)
                                throw new Exception($"Ambiguous maps for the type `{submapper.SerializedType.Name}`");

                            subclassMapToUse2 = submapper;
                        }

                        if (submapper.DiscriminatorFieldName != null)
                        {
                            var discriminatorFieldValueToken = jo[submapper.DiscriminatorFieldName];
                            if (discriminatorFieldValueToken?.Type == JTokenType.String)
                                subDiscriminatorFieldValue = discriminatorFieldValueToken.Value<string>();

                            if (subDiscriminatorFieldValue == null)
                                break;
                        }
                    }
                }

                if (subclassMapToUse2 != null)
                    subclassMapToUse = subclassMapToUse2;

                if (subclassMapToUse2 != null && subDiscriminatorFieldValue != null)
                    throw new Exception("Value of discriminator field not verified by any subclass map.");
            }

            if (subclassMapToUse == null && discriminatorFieldValue != null)
                throw new Exception("Value of discriminator field not verified by any subclass map.");

            // STEP 3: Creating the new value with the correct JsonMapBase class if any
            // - by the way, there is no JsonMapBase when a IAndSubtypes mapper is found
            var classMapToUse2 = subclassMapToUse ?? classMapToUse;
            if (classMapToUse2 != null)
            {
                if (!classMapToUse2.CanCreateNew())
                    throw new Exception($"Cannot create object of type `{classMapToUse2.SerializedType}`");

                value = classMapToUse2.CreateNew();
                serializer.Populate(jo.CreateReader(), value);
            }

            return value;
        }
    }
}