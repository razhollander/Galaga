using System.Collections.Generic;

namespace CoreDomain.Scripts.Services.InputlockService
{
    public interface IInputLockService
    {
        void LockInput(InputLock inputLock);
        void UnlockInput(InputLock inputLock);

        bool IsTagLocked(InputLockTag tag);
        InputLock LockAllInputs();
        InputLock LockAllExcept(List<InputLockTag> inputLockTagsExcept);
        public List<InputLock> GetInputLocks();
    }
}