using System;
using System.Collections.Generic;
using CoreDomain.Scripts.Services.RestService;
using Cysharp.Threading.Tasks;
using Handlers.Serializers.Serializer;
using Services.Logs.Base;

namespace CoreDomain.Scripts.Services.ServerService.Rest
{
    public class RestServerService : IServerService
    {
        private const int PollingMilliseconds = 100;
        private const int RequestTimeoutSeconds = 10;
        private const string ProductionUrl = "";
        private const string DevelopUrl = "";
        private readonly IRestService _restService;
        private readonly ISerializerService _serializerService;

        public RestServerService(
            IRestService restService,
            ISerializerService serializerService)
        {
            _restService = restService;
            _serializerService = serializerService;
        }

        public async UniTask<TResponse> Send<TResponse, TRequest>(TRequest requestData)
        {
            var restResponse = await SendRestRequest(requestData);
            var responseData = HandleResponse<TResponse>(restResponse);

            return responseData;
        }

        private TResponse HandleResponse<TResponse>(RestResponse restResponse)
        {
            TResponse responseData;

            try
            {
                if (typeof(TResponse) == typeof(string))
                {
                    responseData = (TResponse)(object)restResponse.Data;
                }
                else
                {
                    responseData = IsContainsBody(restResponse) ? _serializerService.DeserializeJson<TResponse>(restResponse.Data) : default;
                }
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                responseData = default;
            }

            return responseData;
        }

        private bool IsContainsBody(RestResponse restResponse)
        {
            throw new NotImplementedException();
        }

        private async UniTask<RestResponse> SendRestRequest<TRequest>(TRequest requestData)
        {
            var requestInfo = (RestRequestAttribute) Attribute.GetCustomAttribute(requestData.GetType(), typeof(RestRequestAttribute));
            var serializedData = _serializerService.SerializeJson(requestData);

            var restRequest = new RestRequest
            {
                BaseUrl = false ? ProductionUrl : DevelopUrl,
                EndPoint = requestInfo.EndPoint,
                Body = serializedData,
                Headers = new Dictionary<string, string>(),
                TimeoutSeconds = RequestTimeoutSeconds
            };

            RestResponse response;

            switch (requestInfo.RequestType)
            {
                case RestRequestType.Get:
                    response = await _restService.Get(restRequest);
                    break;
                case RestRequestType.Post:
                    response = await _restService.Post(restRequest);
                    break;
                case RestRequestType.Put:
                    response = await _restService.Put(restRequest);
                    break;
                case RestRequestType.Delete:
                    response = await _restService.Delete(restRequest);
                    break;
                case RestRequestType.Head:
                    response = await _restService.Head(restRequest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LogService.Log(response.ToString());

            return response;
        }

        public async UniTask<TResponse> Listen<TResponse, TRequest>(TRequest requestData)
        {
            while (true)
            {
                var restResponse = await SendRestRequest(requestData);

                if (IsContainsBody(restResponse))
                {
                    return HandleResponse<TResponse>(restResponse);
                }

                await UniTask.Delay(PollingMilliseconds);
            }
        }
    }
}