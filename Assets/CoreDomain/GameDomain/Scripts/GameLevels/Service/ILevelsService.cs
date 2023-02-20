namespace CoreDomain.GameDomain
{
    public interface ILevelsService

    {
        void LoadLevels();
        int GetLevelsAmount();
        LevelData GetLevelData(int levelNumber);
    }
}