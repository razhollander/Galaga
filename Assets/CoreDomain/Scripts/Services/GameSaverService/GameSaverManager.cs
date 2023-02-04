using System.Collections.Generic;
using Handlers;

namespace Managers
{
    public class GameSaverManager : ISaverManager
    {
        #region --- Constants ---

        private const string DOES_HAVE_LAST_SAVE_KEY = "DoesHaveLastSave";

        #endregion


        #region --- Members ---

        private readonly List<ISavableObject> _savedObjectsList = new List<ISavableObject>();

        #endregion


        #region --- Properties ---

        public bool DoesHaveLastSave
        {
            get { return SaveLocallyHandler.LoadBool(DOES_HAVE_LAST_SAVE_KEY); }
            private set { SaveLocallyHandler.SaveBool(DOES_HAVE_LAST_SAVE_KEY, value); }
        }

        #endregion


        #region --- Public Methods ---

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

        #endregion
    }

    public interface ISavableObject
    {
        #region --- Public Methods ---

        void Load();
        void Save();

        #endregion
    }

    public interface ISaverManager
    {
        #region --- Properties ---

        bool DoesHaveLastSave { get; }

        #endregion


        #region --- Public Methods ---

        void LoadGameData();
        void RegisterSavedObject(ISavableObject savableObject);

        void SaveGameData();
        void UnregisterSavedObject(ISavableObject savableObject);

        #endregion
    }
}