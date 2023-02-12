namespace CoreDomain.Services
{
    public interface IGameSaverService
    {
        bool DoesHaveLastSave { get; }
        void LoadGameData();
        void RegisterSavedObject(ISavableObject savableObject);

        void SaveGameData();
        void UnregisterSavedObject(ISavableObject savableObject);
    }
}