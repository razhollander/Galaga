using Managers;
using UnityEngine;

namespace Client
{
    public class ClientInitiator : MonoBehaviour
    {
        #region --- Inspector ---

        [SerializeField] private UpdateManager _updateManager;

        #endregion


        #region --- Mono Override ---

        private void Awake()
        {
            new global::Client.Client(_updateManager);
        }

        #endregion


        #region --- Public Methods ---

        [ContextMenu("Clear Player Prefs")]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            global::Client.Client.Instance.GameSaverManager.SaveGameData();
        }

        #endregion
    }
}