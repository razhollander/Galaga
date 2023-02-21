namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Score
{
    public class ScoreModule : IScoreModule
    {
        private ScoreData _scoreData;

        public int PlayerScore => _scoreData.PlayerScore;

        public ScoreModule()
        {
            _scoreData = new ScoreData();
        }

        public void AddScore(int score)
        {
            _scoreData.PlayerScore += score;
        }
    }
}