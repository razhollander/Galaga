using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Services.GameStates;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LobbyUiModule>().AsSingle().NonLazy();
            Container.BindFactory<LobbyGameStateEnterData, EnterLobbyGameStateCommand, EnterLobbyGameStateCommand.Factory>();
        }
    }
}