using System.Collections.Generic;
using Handlers;

namespace CoreDomain.Services
{
    public class GameSaverService : IGameSaverService
    {
        private const string DoesHaveLastSaveKey = "DoesHaveLastSave";
        private readonly List<ISavableObject> _savedObjectsList = new ();
        
        
        public bool DoesHaveLastSave
        {
            get { return SaveLocallyHandler.LoadBool(DoesHaveLastSaveKey); }
            private set { SaveLocallyHandler.SaveBool(DoesHaveLastSaveKey, value); }
        }
        
        
        public void LoadGameData()
        {
            foreach (var savedObject in _savedObjectsList)
            {
                savedObject.Load();
            }
        }

        public void RegisterSavedObject(ISavableObject savableObject)
        {
            if (_savedObjectsList.Contains(savableObject))
            {
                return;
            }

            _savedObjectsList.Add(savableObject);
        }

        public void SaveGameData()
        {
            DoesHaveLastSave = true;

            foreach (var savedObject in _savedObjectsList)
            {
                savedObject.Save();
            }
        }

        public void UnregisterSavedObject(ISavableObject savableObject)
        {
            if (!_savedObjectsList.Contains(savableObject))
            {
                return;
            }

            _savedObjectsList.Remove(savableObject);
        }
    }
}