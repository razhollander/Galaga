using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public interface IGameState
    {
        GameStateType GameState { get; }
        UniTask EnterState();
        UniTask ExitState();
    }
}