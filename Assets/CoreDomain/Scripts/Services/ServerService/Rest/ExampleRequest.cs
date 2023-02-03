namespace CoreDomain.Scripts.Services.ServerService.Rest
{
    [RestRequest(EndPoint = "example_request", RequestType = RestRequestType.Get)]
    public class ExampleRequest : IBaseServerRequest
    {
        
    }

    public interface IBaseServerRequest
    {
    }
}