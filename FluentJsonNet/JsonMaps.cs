using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FluentJsonNet
{
    public static class JsonMaps
    {
        public static Func<JsonSerializerSettings> GetDefaultSettings(Type[] types)
        {
            var jsonMaps = types
                .Select(t => t.GetTypeInfo())
                .Where(t => typeof(JsonMapBase).GetTypeInfo().IsAssignableFrom(t))
                .Where(t => !t.ContainsGenericParameters && !t.IsAbstract && t.GetConstructor(new Type[0]) != null)
                .Select(t => Activator.CreateInstance(t.AsType()))
                .Cast<JsonMapBase>()
                .ToArray();

            var contractResolver = CreateContractResolver(jsonMaps);
            var converters = CreateConverters(jsonMaps);

            Func<JsonSerializerSettings> defaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Converters = converters,
            };
            return defaultSettings;
        }

        public static Func<JsonSerializerSettings> GetDefaultSettings(object p)
        {
            throw new NotImplementedException();
        }

        private static IList<JsonConverter> CreateConverters([NotNull] IEnumerable<JsonMapBase> jsonMaps)
        {
            if (jsonMaps == null)
                throw new ArgumentNullException(nameof(jsonMaps));

            var result = new JsonMapsConverter(jsonMaps);

            return new List<JsonConverter>(jsonMaps.Count()) { result }.AsReadOnly();
        }

        private static IContractResolver CreateContractResolver([NotNull] IEnumerable<JsonMapBase> jsonMaps)
        {
            return new JsonMapsContractResolver(jsonMaps);
        }
    }
}