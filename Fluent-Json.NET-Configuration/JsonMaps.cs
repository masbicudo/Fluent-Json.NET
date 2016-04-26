using System;
using System.Collections.Generic;
using System.Linq;
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
                .Where(t => typeof(JsonMapBase).IsAssignableFrom(t) && !t.ContainsGenericParameters && !t.IsAbstract && t.GetConstructor(new Type[0]) != null)
                .Select(Activator.CreateInstance)
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