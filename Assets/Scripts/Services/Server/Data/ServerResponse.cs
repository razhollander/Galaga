using System;

namespace Services.Server.Data
{
    [Serializable]
    public class ServerResponse
    {
        public ResponseWebData Data { get; }
        
        public bool IsSuccess => Data.Error == null;

        public ServerResponse()
        {
        }
        
        public ServerResponse(ResponseWebData data)
        {
            Data = data;
        }
    }
}