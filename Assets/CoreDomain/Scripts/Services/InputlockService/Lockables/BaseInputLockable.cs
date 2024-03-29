﻿using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CoreDomain.Scripts.Services.InputlockService.Lockables
{
    public abstract class BaseInputLockable : MonoBehaviour
    {
        [SerializeField] public List<InputLockTag> Tags;
        private IInputLockServiceSubscription _inputLockServiceSubscription;
        public bool IsLocked { get; private set; }

        [Inject]
        private void Inject(IInputLockServiceSubscription inputLockServiceSubscription)
        {
            _inputLockServiceSubscription = inputLockServiceSubscription;
        }

        protected virtual void Awake()
        {
            _inputLockServiceSubscription.SubscribeLockable(this);
        }

        private void OnDestroy()
        {
            _inputLockServiceSubscription?.UnsubscribeLockable(this);
        }

        public void Lock()
        {
            if (IsLocked)
            {
                return;
            }

            LockInternal();
            IsLocked = true;
        }

        public void Unlock()
        {
            if (!IsLocked)
            {
                return;
            }

            UnlockInternal();
            IsLocked = false;
        }

        protected abstract void LockInternal();
        protected abstract void UnlockInternal();
    }
}