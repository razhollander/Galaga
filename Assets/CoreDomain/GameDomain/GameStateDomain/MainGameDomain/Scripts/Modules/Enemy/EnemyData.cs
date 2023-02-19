namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemyData
    {
        public readonly string Id;
        public readonly int Score;

        public EnemyData(string id, int score)
        {
            Id = id;
            Score = score;
        }
    }
}