using CoreDomain.Services.GameStates;
using Services.Logs.Base;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        LogService.Log("InstallBindings");
        Container.BindFactory<LobbyGameStateEnterData, EnterLobbyGameStateCommand, EnterLobbyGameStateCommand.Factory>();
        Container.BindFactory<LobbyGameState, LobbyGameState.Factory>();
    }
}