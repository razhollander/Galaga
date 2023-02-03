using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Services.ServerService
{
    public interface IServerService
    {
        UniTask<TResponse> Send<TResponse, TRequest>(TRequest requestData);
        UniTask<TResponse> Listen<TResponse, TRequest>(TRequest requestData);
    }
}