namespace CoreDomain.GameDomain
{
    public interface ILevelsService

    {
        void LoadLevels();
        int GetLevelsAmount();
        LevelData GetLevel(int levelNumber);
    }
}