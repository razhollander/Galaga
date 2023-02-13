using CoreDomain.Services;
using CoreDomain.Services.GameStates;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiModule : ILobbyUiModule
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly LobbyUiCreator _creator;
        private readonly LobbyUiViewModule _viewModule;

        public LobbyUiModule(IAssetBundleLoaderService assetBundleLoaderService, IStateMachineService stateMachineService)
        {
            _stateMachineService = stateMachineService;
            _creator = new LobbyUiCreator(assetBundleLoaderService);
            _viewModule = new LobbyUiViewModule();
        }

        public void CreateLobbyUi()
        {
            var lobbyUiView = _creator.CreateLobbyUi();
            lobbyUiView.Setup(SwitchToQuickGameState);
            _viewModule.SetupLobbyUiView(lobbyUiView);
        }

        public void DestroyLobbyUi()
        {
            _viewModule.DestroyLobbyUiView();
        }

        private void SwitchToQuickGameState()
        {
            _stateMachineService.SwitchState(new MainGameState(new MainGameStateEnterData("")));
        }
    }
}