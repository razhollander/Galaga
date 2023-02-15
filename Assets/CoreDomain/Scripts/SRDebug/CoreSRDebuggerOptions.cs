using System.ComponentModel;
using CoreDomain.Services;
using SRDebugger;
using UnityEngine;

namespace CoreDomain.Scripts.SRDebug
{
    public class CoreSrDebuggerOptions
    {
        private float _gameSpeed = 1f;

        public CoreSrDebuggerOptions()
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
            global::SRDebug.Instance.AddOptionContainer(this);
        }
    }
}
