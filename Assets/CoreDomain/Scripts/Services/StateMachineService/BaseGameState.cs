using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;

public abstract class BaseGameState<T> : IGameState where T: IGameStateEnterData
{
    protected T EnterData { get; }
    
    protected BaseGameState(T enterData)
    {
        EnterData = enterData;
    }

    public abstract GameStateType GameState { get; }
    public abstract UniTask EnterState();
    public abstract UniTask ExitState();
}
