using System.Collections.Generic;

namespace CoreDomain.Scripts.Services.RestService
{
    public struct RestRequest
    {
        public string BaseUrl;
        public string EndPoint;
        public string Body;
        public Dictionary<string, string> Headers;
        public int TimeoutSeconds;
    }
}