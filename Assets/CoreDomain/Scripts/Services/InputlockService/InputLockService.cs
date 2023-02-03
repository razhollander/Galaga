using System;
using System.Collections.Generic;
using System.Linq;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Services.InputlockService.Lockables;
using Services.Logs.Base;

namespace CoreDomain.Scripts.Services.InputlockService
{
    public class InputLockService : IInputLockService, IInputLockServiceSubscription
    {
        private readonly Dictionary<InputLockTag, HashSet<string>> _locks = new();
        private readonly Dictionary<string, InputLock> _idToInputLock = new();
        private readonly List<BaseInputLockable> _inputLockSubscribers = new();
#if UNITY_EDITOR
        public static InputLockService _inputLockService;
        public event Action LocksUpdated;
#endif
        public InputLockService()
        {
#if UNITY_EDITOR
            _inputLockService = this;
#endif
            foreach (var tags in Enum.GetValues(typeof(InputLockTag)))
            {
                _locks.Add((InputLockTag) tags, new HashSet<string>());
            }
        }

        public List<InputLock> GetInputLocks()
        {
            return _idToInputLock.Values.ToList();
        }

        public void SubscribeLockable(BaseInputLockable inputLockable)
        {
            _inputLockSubscribers.Add(inputLockable);
            UpdateLockable(inputLockable);
        }

        public void UnsubscribeLockable(BaseInputLockable inputLockable)
        {
            _inputLockSubscribers.Remove(inputLockable);
        }

        public bool IsTagLocked(InputLockTag tag)
        {
            return _locks[tag].Count > 0;
        }

        public InputLock LockAllInputs()
        {
            var inputLockTags = Enum.GetValues(typeof(InputLockTag)).OfType<InputLockTag>().ToList();
            
            var inputLock = new InputLock(inputLockTags);  
            LockInput(inputLock);
            return inputLock;
        }

        public InputLock LockAllExcept(List<InputLockTag> inputLockTagsExcept)
        {
            var inputLockTags = Enum.GetValues(typeof(InputLockTag)).OfType<InputLockTag>().ToList();

            foreach (var exceptionTag in inputLockTagsExcept)
            {
                inputLockTags.Remove(exceptionTag);
            }

            var inputLock = new InputLock(inputLockTags);
            LockInput(inputLock);
            return inputLock;
        }

        public void LockInput(InputLock inputLock)
        {
            Log(nameof(LockInput), $"LockOwner: {inputLock.LockOwner}, lockTags: {inputLock.InputLockTags.ToStringItems()}");

            foreach (var lockType in inputLock.InputLockTags)
            {
                if (_locks[lockType].Contains(inputLock.Guid))
                {
                    LogService.Log($"The lock {inputLock.Guid} is already locked");
                    return;
                }

                _locks[lockType].Add(inputLock.Guid);
            }

            _idToInputLock.Add(inputLock.Guid, inputLock);
            UpdateLockables();

#if UNITY_EDITOR
            LocksUpdated?.Invoke();
#endif
        }

        public void UnlockInput(InputLock inputLock)
        {
            Log(nameof(UnlockInput), $"LockOwner: {inputLock.LockOwner}, lockTags: {inputLock.InputLockTags.ToStringItems()}");

            foreach (var lockType in inputLock.InputLockTags)
            {
                if (!_locks[lockType].Contains(inputLock.Guid))
                {
                    LogService.Log($"You are trying to unlock {inputLock.ToString()} but no such inputlock exists");
                    return;
                }

                _locks[lockType].Remove(inputLock.Guid);
            }

            _idToInputLock.Remove(inputLock.Guid);

            UpdateLockables();
#if UNITY_EDITOR
            LocksUpdated?.Invoke();
#endif
        }

        private void UpdateLockables()
        {
            foreach (var inputLockable in _inputLockSubscribers)
            {
                UpdateLockable(inputLockable);
            }
        }

        private void UpdateLockable(BaseInputLockable inputLockable)
        {
            var shouldLock = ShouldLockInputLockable(inputLockable);

            if (shouldLock)
            {
                inputLockable.Lock();
            }
            else
            {
                inputLockable.Unlock();
            }
        }

        private bool ShouldLockInputLockable(BaseInputLockable inputLockable)
        {
            var shouldLock = false;

            foreach (var tag in inputLockable.Tags)
            {
                var isLocked = _locks[tag].Count != 0;

                if (isLocked)
                {
                    shouldLock = true;
                    break;
                }
            }

            return shouldLock;
        }

        private void Log(string methodName, string methodContent = null)
        {
            LogService.LogTag($"{methodName} , {methodContent}", LogTagType.Inputs);
        }
    }
}