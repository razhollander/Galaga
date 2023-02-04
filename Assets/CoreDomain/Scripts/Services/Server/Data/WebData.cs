using Newtonsoft.Json;

namespace Services.Server.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class WebData
    {
        [JsonProperty("id")] private string _id = "1";
        
        [JsonProperty("jsonrpc")] private string _jsonRPC = "2.0";
    }
}