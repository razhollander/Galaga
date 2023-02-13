using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;

public class EnterLobbyGameStateCommand : CommandOneParameter<LobbyGameStateEnterData, EnterLobbyGameStateCommand>
{
    private readonly ILobbyUiModule _lobbyUiModule;
    private readonly LobbyGameStateEnterData _lobbyGameStateEnterData;
    private readonly ISceneLoaderService _sceneLoaderService;

    public EnterLobbyGameStateCommand(LobbyGameStateEnterData lobbyGameStateEnterData, ISceneLoaderService sceneLoaderService, ILobbyUiModule lobbyUiModule)
    {
        _lobbyGameStateEnterData = lobbyGameStateEnterData;
        _sceneLoaderService = sceneLoaderService;
        _lobbyUiModule = lobbyUiModule;
    }

    public override async UniTask Execute()
    {
        await _sceneLoaderService.TryLoadScene(SceneName.Lobby);

        //await _lobbyUiModule.CreateLobbyUi();
    }
}