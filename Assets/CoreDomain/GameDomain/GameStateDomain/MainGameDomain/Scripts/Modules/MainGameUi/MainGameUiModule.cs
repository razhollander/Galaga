using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiModule : IMainGameUiModule
    {
        private readonly JoystickDraggedCommand.Factory _joystickDraggedCommandFactory;
        private readonly CommandSync<ShootButtonClickedCommand>.Factory _shootButtonClickedCommandFactory;
        private readonly MainGameUiCreator _creator;
        private readonly MainGameUiViewModule _viewModule;

        public MainGameUiModule(IAssetBundleLoaderService assetBundleLoaderService, JoystickDraggedCommand.Factory joystickDraggedCommandFactory, ShootButtonClickedCommand.Factory shootButtonClickedCommandFactory)
        {
            _joystickDraggedCommandFactory = joystickDraggedCommandFactory;
            _shootButtonClickedCommandFactory = shootButtonClickedCommandFactory;
            _creator = new MainGameUiCreator(assetBundleLoaderService);
            _viewModule = new MainGameUiViewModule();
        }

        public void UpdateScore(int newScore)
        {
            _viewModule.UpdateScore(newScore);
        }

        public void CreateMainGameUi()
        {
            var mainGameUiView = _creator.CreateMainGameUi();
            mainGameUiView.Setup(OnShootButtonClicked, OnJoystickInputChanged);
            _viewModule.SetupMainGameUiView(mainGameUiView);
        }

        private void OnJoystickInputChanged(float dragValue)
        {
            _joystickDraggedCommandFactory.Create(dragValue).Execute();
        }

        private void OnShootButtonClicked()
        {
            _shootButtonClickedCommandFactory.Create().Execute();
        }

        public void DestroyMainGameUi()
        {
            _viewModule.DestroyMainGameUiView();
        }
    }
}