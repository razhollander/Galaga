using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreDomain.Scripts.Services.InputlockService.Lockables
{
    [RequireComponent(typeof(BaseRaycaster))]
    public class RaycasterInputLocker : BaseInputLockable
    {
        private BaseRaycaster _baseRaycaster;

        protected override void Awake()
        {
            base.Awake();
            _baseRaycaster = GetComponent<BaseRaycaster>();
        }

        protected override void LockInternal()
        {
            _baseRaycaster.enabled = false;
        }

        protected override void UnlockInternal()
        {
            _baseRaycaster.enabled = true;
        }
    }
}