using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public interface IStateMachineService
    {
        IGameState CurrentState();
        void EnterInitialGameState(IGameState initialState);
        UniTask SwitchState(IGameState newState);
    }
}