using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CoreDomain.Scripts.Services.RestService
{
    public class UnityWebRequestRestService : IRestService
    {
        public async UniTask<RestResponse> Get(RestRequest request)
        {
            return await Request(() => UnityWebRequest.Get(GetUri(request)), request);
        }

        public async UniTask<RestResponse> Post(RestRequest request)
        {
            return await Request(() => UnityWebRequest.Post(GetUri(request), request.Body), request);
        }

        public async UniTask<RestResponse> Delete(RestRequest request)
        {
            return await Request(() => UnityWebRequest.Delete(GetUri(request)), request);
        }

        public async UniTask<RestResponse> Put(RestRequest request)
        {
            return await Request(() => UnityWebRequest.Put(GetUri(request), request.Body), request);
        }

        public async UniTask<RestResponse> Head(RestRequest request)
        {
            return await Request(() => UnityWebRequest.Head(GetUri(request)), request);
        }

        private async UniTask<RestResponse> Request(Func<UnityWebRequest> requestToServer, RestRequest restRequest)
        {
            using UnityWebRequest webRequest = requestToServer.Invoke();
            webRequest.timeout = restRequest.TimeoutSeconds;
            
            await webRequest.SendWebRequest().ToUniTask();

            return new RestResponse
            {
                Data = Encoding.UTF8.GetString(webRequest.downloadHandler.data),
                Error = webRequest.error,
                StatusCode = (int) webRequest.responseCode,
                Headers = webRequest.GetResponseHeaders()
            };
        }

        private string GetUri(RestRequest request)
        {
            return $"{request.BaseUrl}/{request.EndPoint}";
        }
    }
}