using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LobbyUiModule>().AsSingle().NonLazy();
        }
    }
}