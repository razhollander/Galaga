namespace CoreDomain.Scripts.Utils.Pools
{
    public interface IPool<T> where T : IPoolable
    {
        void InitPool();
        T Spawn();
        void Despawn(T obj);
    }
}