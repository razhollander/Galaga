using UnityEngine.EventSystems;

namespace CoreDomain.Scripts.Services.InputlockService.Lockables
{
    public class EventTriggerInputLockable : BaseInputLockable
    {
        private EventTrigger _eventTrigger;

        protected override void Awake()
        {
            base.Awake();
            _eventTrigger = GetComponent<EventTrigger>();
        }

        protected override void LockInternal()
        {
            _eventTrigger.enabled = false;
        }

        protected override void UnlockInternal()
        {
            _eventTrigger.enabled = true;
        }
    }
}