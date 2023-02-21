namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public interface IMainGameUiModule
    {
        void UpdateScore(int newScore);
        void CreateMainGameUi();
        void Dispose();
    }
}