using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Services.RestService
{
    public interface IRestService
    {
        UniTask<RestResponse> Get(RestRequest request);
        UniTask<RestResponse> Post(RestRequest request);
        UniTask<RestResponse> Delete(RestRequest request);
        UniTask<RestResponse> Put(RestRequest request);
        UniTask<RestResponse> Head(RestRequest request);
    }
}