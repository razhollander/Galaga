using System;

namespace CoreDomain.Utils.Pools
{
    public interface IPoolable
    {
        Action Despawn { set; }
        void OnSpawned();
        void OnDespawned();
    }
}