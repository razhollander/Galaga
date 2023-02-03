using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreDomain.Scripts.Services.RestService
{
    public struct RestResponse
    {
        public int StatusCode;
        public string Data;
        public string Error;
        public Dictionary<string, string> Headers;

        public override string ToString()
        {
            var headersString = string.Join("|", Headers?.Select(kv => $"Key: {kv.Key}, Value: {kv.Value})") ?? Array.Empty<string>());
            return $"Server Response - Code: {StatusCode}, Error: {Error}, Data: {Data}, Headers: {headersString}";
        }
    }
}