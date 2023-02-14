using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using Services.Logs.Base;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            LogService.Log("Install lobby");
            Container.BindInterfacesTo<LobbyUiModule>().AsSingle().NonLazy();
        }
    }
}