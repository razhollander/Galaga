namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Score
{
    public interface IScoreModule
    {
        void AddScore(int score);
        int PlayerScore { get; }
    }
}