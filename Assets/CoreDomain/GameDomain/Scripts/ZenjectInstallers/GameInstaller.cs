using CoreDomain.Services.GameStates;
using Zenject;

namespace CoreDomain.GameDomain
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<LobbyGameState, LobbyGameState.Factory>();
            Container.BindFactory<MainGameStateEnterData, MainGameState, MainGameState.Factory>();
            Container.BindInterfacesTo<LevelsService>().AsSingle().NonLazy();
        }
    }
}