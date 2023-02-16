using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiModule : IMainGameUiModule
    {
        private readonly JoystickDraggedCommand.Factory _joystickDraggedCommandFactory;
        private readonly MainGameUiCreator _creator;
        private readonly MainGameUiViewModule _viewModule;

        public MainGameUiModule(IAssetBundleLoaderService assetBundleLoaderService, JoystickDraggedCommand.Factory joystickDraggedCommandFactory)
        {
            _joystickDraggedCommandFactory = joystickDraggedCommandFactory;
            _creator = new MainGameUiCreator(assetBundleLoaderService);
            _viewModule = new MainGameUiViewModule();
        }

        public void CreateMainGameUi()
        {
            var mainGameUiView = _creator.CreateMainGameUi();
            mainGameUiView.Setup(OnShootButtonClicked, OnJoystickInputChanged);
            _viewModule.SetupMainGameUiView(mainGameUiView);
        }

        private void OnJoystickInputChanged(float dragValue)
        {
            _joystickDraggedCommandFactory.Create(dragValue).Execute().Forget();
        }

        private void OnShootButtonClicked()
        {
            
        }

        public void DestroyMainGameUi()
        {
            _viewModule.DestroyMainGameUiView();
        }
    }
}