namespace CoreDomain.Services.GameStates
{
    public class MainGameStateEnterData : IGameStateEnterData
    {
        public readonly string PlayerName;

        public MainGameStateEnterData(string playerName)
        {
            PlayerName = playerName;
        }
    }
}