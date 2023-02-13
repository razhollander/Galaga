using Systems;
using Handlers;
using CoreDomain;
using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using UnityEngine;

namespace Client
{
    public class Client : IClient
    {
        #region --- Properties ---

        public static IClient Instance { get; private set; }
        public PopupsManager PopupsManager { get; }
        public IStateMachineService StateMachineService { get; }
        public UpdateSubscriptionService UpdateSubscriptionService { get; }
        public AssetBundleLoaderService AssetBundleLoaderService { get; private set; }

        public BroadcastService Broadcaster { get; private set; }
        public CameraManager CameraManager { get; private set; }
        public GameInputActions GameInputActions { get; private set; }
        public IGameSaverService GameSaverService { get; private set; }

        #endregion


        #region --- Construction ---

        public Client(UpdateSubscriptionService updateSubscriptionService)
        {
            SetupSingleton(this);
            UpdateSubscriptionService = updateSubscriptionService;
            
            UpdateApplicationSettings();
            SetupSystems();

            PopupsManager = new PopupsManager(this);
            //StateMachine = new StateMachine(this, new StartScreenState());
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
            GameSaverService = new GameSaverService();

            Broadcaster = new BroadcastService();
            AssetBundleLoaderService = new AssetBundleLoaderService();

            GameInputActions = new GameInputActions();
            GameInputActions.Enable();
        }

        #endregion
    }

    public interface IClient
    {
        #region --- Properties ---

        AssetBundleLoaderService AssetBundleLoaderService { get; }
        BroadcastService Broadcaster { get; }
        CameraManager CameraManager { get; }
        GameInputActions GameInputActions { get; }
        IGameSaverService GameSaverService { get; }
        PopupsManager PopupsManager { get; }
        IStateMachineService StateMachineService { get; }
        UpdateSubscriptionService UpdateSubscriptionService { get; }

        #endregion
    }
}