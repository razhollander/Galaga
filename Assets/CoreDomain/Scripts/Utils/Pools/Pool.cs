using System.Collections.Generic;
using CoreDomain.Services;
using UnityEngine;
using Zenject;

namespace CoreDomain.Scripts.Utils.Pools
{
    public abstract class BasePool<TPoolable, TV> : IPool<TPoolable> where TPoolable : IPoolable
    {
        private readonly int _increaseStepAmount;
        private Queue<TPoolable> _pool;
        private readonly int _initialAmount;

        [Inject]
        public void Inject(ICameraService cameraService)
        {
            Debug.Log("BasePool Inject");
        }
        
        public BasePool(PoolData poolData)
        {
            Debug.Log("BasePool");
            _increaseStepAmount = poolData.IncreaseStepAmount;
            _initialAmount = poolData.InitialAmount;
        }

        public virtual void InitPool()
        {
            _pool = new();
            AddInstancesToQueue(_initialAmount);
        }

        private void AddInstancesToQueue(int instancesAmount)
        {
            var poolableInstances = CreatePoolableInstances(instancesAmount);
            poolableInstances.ForEach(poolable => _pool.Enqueue(poolable));
        }
        
        protected abstract List<TPoolable> CreatePoolableInstances(int instancesAmount);
        
        public TPoolable Spawn()
        {
            TPoolable obj;

            if (_pool.Count <= 0)
            {
                AddInstancesToQueue(_increaseStepAmount);
            }
            
            obj = _pool.Dequeue();
            obj.InitializePoolable();

            return obj;
        }

        public void Despawn(TPoolable obj)
        {
            obj.ResetPoolable();
            _pool.Enqueue(obj);
        }
        
        public void Despawn(TPoolable obj)
        {
            obj.ResetPoolable();
            _pool.Enqueue(obj);
        }
        public class Factory: PlaceholderFactory<PoolData, TV>
        {
        
        }
    }
}