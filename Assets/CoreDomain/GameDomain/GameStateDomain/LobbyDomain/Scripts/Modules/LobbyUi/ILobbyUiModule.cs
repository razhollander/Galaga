namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public interface ILobbyUiModule
    {
        void CreateLobbyUi(int levelsAmount);
        void DestroyLobbyUi();
    }
}