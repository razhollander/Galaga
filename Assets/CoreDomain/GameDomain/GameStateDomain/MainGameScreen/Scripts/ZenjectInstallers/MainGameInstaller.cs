using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.Services;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class MainGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            LogService.Log("Install main game");
            Container.BindInterfacesTo<MainGameUiModule>().AsSingle().NonLazy();
        }
    }
}