using Systems;
using GameStates;
using Handlers;
using Managers;
using GameStates;
using UnityEngine;

namespace Client
{
    public class Client : IClient
    {
        #region --- Properties ---

        public static IClient Instance { get; private set; }
        public PopupsManager PopupsManager { get; }
        public StateMachine StateMachine { get; }
        public UpdateManager UpdateManager { get; }
        public AssetBundleSystem AssetBundleSystem { get; private set; }

        public BroadcastSystem Broadcaster { get; private set; }
        public CameraManager CameraManager { get; private set; }
        public GameInputActions GameInputActions { get; private set; }
        public ISaverManager GameSaverManager { get; private set; }

        #endregion


        #region --- Construction ---

        public Client(UpdateManager updateManager)
        {
            SetupSingleton(this);
            UpdateManager = updateManager;
            
            UpdateApplicationSettings();
            SetupSystems();

            PopupsManager = new PopupsManager(this);
            StateMachine = new StateMachine(this, new StartScreenState());
        }

        #endregion


        #region --- Private Methods ---

        private void UpdateApplicationSettings()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        
        private static void SetupSingleton(IClient client)
        {
            Instance = client;
        }

        private void SetupSystems()
        {
            CameraManager = new CameraManager();
            GameSaverManager = new GameSaverManager();

            Broadcaster = new BroadcastSystem();
            AssetBundleSystem = new AssetBundleSystem();

            GameInputActions = new GameInputActions();
            GameInputActions.Enable();
        }

        #endregion
    }

    public interface IClient
    {
        #region --- Properties ---

        AssetBundleSystem AssetBundleSystem { get; }
        BroadcastSystem Broadcaster { get; }
        CameraManager CameraManager { get; }
        GameInputActions GameInputActions { get; }
        ISaverManager GameSaverManager { get; }
        PopupsManager PopupsManager { get; }
        StateMachine StateMachine { get; }
        UpdateManager UpdateManager { get; }

        #endregion
    }
}