using System.Text;
using System.Threading.Tasks;
using CoreDomain.Services;
using Services.Server.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Server
{
    public class ServerService : IServerService
    {
        private const int NoConnectionError = 502;
        private const int Timeout = 10;
        private const string AuthorizationTokenKey = "Authorization";
        private const string AuthorizationTokenValuePrefix = "Bearer ";
        private const string ContentTypeKey = "Content-Type";
        private const string ContentTypeValue = "application/json";

        public string HostAddress { private get; set; }
        public string AuthorizationToken { private get; set; }
        
        public async Task<ServerResponse> Send(ServerRequest request)
        {
            var serializer = new Handlers.Serializers.Serializer.SerializerService();
            var body = serializer.SerializeJson(request.PayloadData);

            using var webRequest = UnityWebRequest.Post(HostAddress, new WWWForm());
            
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
            webRequest.timeout = Timeout;
            webRequest.SetRequestHeader(ContentTypeKey, ContentTypeValue);
            webRequest.SetRequestHeader(AuthorizationTokenKey, $"{AuthorizationTokenValuePrefix}{AuthorizationToken}");
            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
                await Task.Delay(10);
            }

            LogService.Log($"URL REQUEST:{HostAddress} \n request:\n{body} \n response:\n{webRequest.downloadHandler.text}");

            ResponseWebData responseWebData;
            if (string.IsNullOrEmpty(webRequest.error))
            {
                responseWebData = serializer.DeserializeJson<ResponseWebData>(webRequest.downloadHandler.text);
            }
            else
            {
                responseWebData = new ResponseWebData(null, new ErrorData(NoConnectionError, webRequest.error, null));
            }

            return new ServerResponse(responseWebData);
        }
    }
}