
using System;

namespace CoreDomain.Scripts.Utils.Pools
{
    public interface IPoolable
    {
        Action Despawn { set; get; }
        void OnSpawned();
        void OnDespawned();
    }
}