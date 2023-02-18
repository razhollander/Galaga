namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemyData
    {
        public readonly string Id;
        public readonly string Score;

        public EnemyData(string id, string score)
        {
            Id = id;
            Score = score;
        }
    }
}