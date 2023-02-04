using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UpdateManager : MonoBehaviour
    {
        #region --- Members ---

        private List<IUpdatable> _updatablesList = new List<IUpdatable>();

        #endregion


        #region --- Mono Override ---

        private void Awake()
        {
            _updatablesList = new List<IUpdatable>();
        }

        private void Update()
        {
            var numValues = _updatablesList.Count;

            if (numValues <= 0)
            {
                return;
            }

            for (var index = numValues - 1; index > -1; index--)
            {
                var updated = _updatablesList[index];
                updated?.ManagedUpdate();
            }
        }

        #endregion


        #region --- Public Methods ---

        public void RegisterUpdatable(IUpdatable updatable)
        {
            if (_updatablesList == null || _updatablesList.Contains(updatable))
            {
                return;
            }

            _updatablesList.Add(updatable);
        }

        public void UnregisterUpdatable(IUpdatable updatable)
        {
            if (_updatablesList == null || !_updatablesList.Contains(updatable))
            {
                return;
            }

            _updatablesList.Remove(updatable);
        }

        #endregion


        #region --- Inner Classes ---

        public interface IUpdatable
        {
            #region --- Public Methods ---

            void ManagedUpdate();

            #endregion
        }

        #endregion
    }
}