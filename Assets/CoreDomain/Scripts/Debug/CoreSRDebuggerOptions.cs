using System.ComponentModel;
using Services.Logs.Base;
using SRDebugger;
using UnityEngine;

namespace CoreDomain.Scripts.Debug
{
    public class CoreSRDebuggerOptions
    {
        private float _gameSpeed = 1f;

        public CoreSRDebuggerOptions()
        {
#if DEBUG_ENABLED || UNITY_EDITOR
            CreateCheats();
#endif
        }

        [Category("Cheats")]
        [NumberRange(0, 5)]
        public float GameSpeed
        {
            get => _gameSpeed;
            set
            {
                _gameSpeed = value;
                Time.timeScale = _gameSpeed;
            }
        }

        [Category("Utilities")]
        public void ClearPlayerPrefs()
        {
            LogService.Log("Clearing PlayerPrefs");
            PlayerPrefs.DeleteAll();
        }

        private void CreateCheats()
        {
            SRDebug.Instance.AddOptionContainer(this);
        }
    }
}
