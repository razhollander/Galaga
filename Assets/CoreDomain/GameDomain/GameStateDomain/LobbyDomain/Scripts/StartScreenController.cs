using Client;
using GameStates;
using UnityEngine;

namespace Features.StartScreen
{
    public class StartScreenController
    {
        #region --- Constants ---

        private const string START_SCREEN_ASSET_NAME = "StartScreenCanvas";
        private const string START_SCREEN_BUNDLE_PATH = "StartScreenState/StartScreenCanvas";

        #endregion


        #region --- Members ---

        private readonly IClient _client;
        private StartScreenView _view;

        #endregion


        #region --- Construction ---

        public StartScreenController()
        {
            _client = Client.Client.Instance;
        }

        #endregion


        #region --- Public Methods ---

        public void CreateStartScreen()
        {
            var startScreenGO =
                GameObject.Instantiate(
                    _client.AssetBundleSystem.LoadAssetFromBundle<GameObject>(START_SCREEN_BUNDLE_PATH,
                        START_SCREEN_ASSET_NAME));

            _view = startScreenGO.GetComponent<StartScreenView>();
            _view.Setup(_client, StartNewGame, StartGameFromLastSave);
        }

        public void DestroyStartScreen()
        {
            Object.Destroy(_view.gameObject);
        }

        #endregion


        #region --- Private Methods ---

        private void StartGameFromLastSave()
        {
            _client.StateMachineService.SwitchState(new MainGameState(true));
        }

        private void StartNewGame()
        {
            _client.StateMachineService.SwitchState(new MainGameState());
        }

        #endregion
    }
}