using CoreDomain.Services;
using CoreDomain.Services.GameStates;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiModule : ILobbyUiModule
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly LobbyUiCreator _creator;
        private readonly LobbyUiViewModule _viewModule; // raz need this view module?
        private readonly MainGameState.Factory _mainGameStateFactory;

        public LobbyUiModule(IAssetBundleLoaderService assetBundleLoaderService, IStateMachineService stateMachineService, MainGameState.Factory mainGameStateFactory)
        {
            _stateMachineService = stateMachineService;
            _creator = new LobbyUiCreator(assetBundleLoaderService);
            _viewModule = new LobbyUiViewModule();
            _mainGameStateFactory = mainGameStateFactory;
        }

        public void CreateLobbyUi(int levelsAmount)
        {
            var lobbyUiView = _creator.CreateLobbyUi();
            lobbyUiView.Setup(SwitchToQuickGameState, levelsAmount);
            _viewModule.SetupLobbyUiView(lobbyUiView);
        }

        public void DestroyLobbyUi()
        {
            _viewModule.DestroyLobbyUiView();
        }

        private void SwitchToQuickGameState()
        {
            _stateMachineService.SwitchState(_mainGameStateFactory.Create(new MainGameStateEnterData(_viewModule.GetPlayerName(), _viewModule.GetSelectedLevel())));
        }
    }
}