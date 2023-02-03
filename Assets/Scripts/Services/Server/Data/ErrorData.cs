using Newtonsoft.Json;

namespace Services.Server.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorData
    {
        [JsonProperty("code")] public int Code { get; private set; }
        [JsonProperty("message")] public string Message { get; private set; }
        [JsonProperty("data")] public DetailsData Data { get; private set; }

        [JsonObject(MemberSerialization.OptIn)]
        public class DetailsData
        {
            [JsonProperty("details")] public string Details { get; private set; }
        }

        public ErrorData()
        {
        }
        
        public ErrorData(int code, string message, DetailsData data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}