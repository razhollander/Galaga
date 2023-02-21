using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public interface IStateInitiator<T> where T : class, IGameStateEnterData
    {
        UniTask EnterState(T stateEnterData = null);
        UniTask ExitState();
    }
}