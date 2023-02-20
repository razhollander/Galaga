namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public struct EnemyData
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