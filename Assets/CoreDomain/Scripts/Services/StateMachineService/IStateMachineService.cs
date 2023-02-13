using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public interface IStateMachineService
    {
        void EnterInitialGameState(IGameState initialState);
        UniTask SwitchState(IGameState newState);
    }
}