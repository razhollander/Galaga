using System.Collections.Generic;
using UnityEngine;

namespace CoreDomain.Services
{
    public class UpdateSubscriptionService : MonoBehaviour, IUpdateSubscriptionService
    {
        private List<IUpdatable> _updatablesList = new ();
        
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
    }
}