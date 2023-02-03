using System;
using Newtonsoft.Json;

namespace Modules.Analytics.Editor.Data
{
    [Serializable]
    public class AnalyticPropertyData
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("type")] public string Type;
        [JsonProperty("default")] public object DefaultValue;
        [JsonProperty("constructor_property")] public bool ConstructorProperty;

    }
}