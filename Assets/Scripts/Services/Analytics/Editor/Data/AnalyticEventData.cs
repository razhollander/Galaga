using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Modules.Analytics.Editor.Data
{
    [Serializable]
    public class AnalyticEventData
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("parameters")] public List<AnalyticPropertyData> Parameters;

    }
}