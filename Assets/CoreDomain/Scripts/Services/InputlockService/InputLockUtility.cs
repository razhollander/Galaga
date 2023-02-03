using UnityEngine;
using Zenject;

namespace CoreDomain.Scripts.Services.InputlockService
{
    public class InputLockUtility : MonoBehaviour
    {
        private InputLock _allInputInputLock;

        private InputLock _inputLock;
        [Inject] private InputLockService _inputLockService;

        public void LockAllInput()
        {
            if (_allInputInputLock != null)
            {
                return;
            }

            _allInputInputLock = _inputLockService.LockAllInputs();
        }

        public void UnlockAllInput()
        {
            if (_allInputInputLock == null)
            {
                return;
            }

            _inputLockService.UnlockInput(_allInputInputLock);
            _allInputInputLock = null;
        }
    }
}