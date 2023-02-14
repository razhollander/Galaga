using CoreDomain.Services;
using CoreDomain.Services.GameStates;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiModule : IMainGameUiModule
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly MainGameUiCreator _creator;
        private readonly MainGameUiViewModule _viewModule;

        public MainGameUiModule(IAssetBundleLoaderService assetBundleLoaderService, IStateMachineService stateMachineService)
        {
            _stateMachineService = stateMachineService;
            _creator = new MainGameUiCreator(assetBundleLoaderService);
            _viewModule = new MainGameUiViewModule();
        }

        public void CreateMainGameUi()
        {
            var mainGameUiView = _creator.CreateMainGameUi();
            mainGameUiView.SetCallbacks(SwitchToQuickGameState);
            _viewModule.SetupMainGameUiView(mainGameUiView);
        }

        public void DestroyMainGameUi()
        {
            _viewModule.DestroyMainGameUiView();
        }

        private void SwitchToQuickGameState()
        {
            _stateMachineService.SwitchState(new MainGameState(new MainGameStateEnterData(_viewModule.GetPlayerName())));
        }
    }
}