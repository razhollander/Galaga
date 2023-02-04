using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.Server.Data
{
    public class ResponseWebData : WebData
    {
        [JsonProperty("result")] public Dictionary<string, JObject> Records { get; private set; }
        
        [JsonProperty("error")] public ErrorData Error { get; private set; }
        
        public ResponseWebData()
        {
        }
        
        public ResponseWebData(Dictionary<string, JObject> records, ErrorData error)
        {
            Records = records;
            Error = error;
        }
    }
}