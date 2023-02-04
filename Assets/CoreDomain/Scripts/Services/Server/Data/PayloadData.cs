using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.Server.Data
{
    [Serializable]
    public abstract class PayloadData : WebData
    {
        [JsonProperty("method")] public abstract string MethodName { get; }
        [JsonProperty("params")] public Dictionary<string, object> Parameters { get; } = new();
    }
}