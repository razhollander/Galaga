using CoreDomain.Scripts.Services.InputlockService.Lockables;

namespace CoreDomain.Scripts.Services.InputlockService
{
    public interface IInputLockServiceSubscription
    {
        void SubscribeLockable(BaseInputLockable inputLockable);
        void UnsubscribeLockable(BaseInputLockable inputLockable);
    }
}