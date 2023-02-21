using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public abstract class BaseGameState<T> : IGameState where T : IGameStateEnterData
    {
        public T EnterData { get; }

        protected BaseGameState(T enterData)
        {
            EnterData = enterData;
        }

        public abstract GameStateType GameState { get; }
        public abstract UniTask EnterState();
        public abstract UniTask ExitState();
    }
}