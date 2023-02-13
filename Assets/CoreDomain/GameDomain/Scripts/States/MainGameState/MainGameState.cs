using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
     public class MainGameState : BaseGameState<MainGameStateEnterData>
     {
         //private readonly EnemiesController _enemiesController;
         //private readonly GameLogicController _gameLogicController;
         //private readonly IClient _client;
         //private readonly PlayerController _playerController;
    
         public override GameStateType GameState => GameStateType.MainGame;
    
         public MainGameState(MainGameStateEnterData gameStateEnterData) : base(gameStateEnterData)
         {
             //_client = Client.Client.Instance;
             //_isFromLastSave = isFromLastSave;
             //_gameLogicController = new GameLogicController(_client);
             //_playerController = new PlayerController(_client);
             //_enemiesController = new EnemiesController(_client);
         }
         
         public override async UniTask EnterState()
         {
             //_playerController.Setup();
             //_enemiesController.Setup();
         }
    
         public override async UniTask ExitState()
         {
             //_gameLogicController.Dispose();
             //_playerController.Dispose();
             //_enemiesController.Dispose();
         }
    }
}