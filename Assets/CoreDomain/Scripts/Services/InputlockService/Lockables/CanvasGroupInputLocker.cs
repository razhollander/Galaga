using UnityEngine;

namespace CoreDomain.Scripts.Services.InputlockService.Lockables
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupInputLocker : BaseInputLockable
    {
        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void LockInternal()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        protected override void UnlockInternal()
        {
            _canvasGroup.blocksRaycasts = true;
        }
    }
}