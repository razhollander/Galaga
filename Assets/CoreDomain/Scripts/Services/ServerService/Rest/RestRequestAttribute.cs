using System;

namespace CoreDomain.Scripts.Services.ServerService.Rest
{
    public class RestRequestAttribute : Attribute
    {
        public string EndPoint;
        public RestRequestType RequestType;
    }
}