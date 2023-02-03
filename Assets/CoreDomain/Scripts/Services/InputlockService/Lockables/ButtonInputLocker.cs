using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.Scripts.Services.InputlockService.Lockables
{
    [RequireComponent(typeof(Button))]
    public class ButtonInputLocker : BaseInputLockable
    {
        private Button _button;

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        protected override void LockInternal()
        {
            _button.enabled = false;
        }

        protected override void UnlockInternal()
        {
            _button.enabled = true;
        }
    }
}