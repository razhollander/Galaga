using System;

namespace CoreDomain.Services.GameStates
{
    [Serializable]
    public class MainGameStateEnterData : IGameStateEnterData
    {
        public string PlayerName;
        public int Level;

        public MainGameStateEnterData(string playerName, int level)
        {
            PlayerName = playerName;
            Level = level;
        }
    }
}