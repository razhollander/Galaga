namespace CoreDomain.Scripts.Utils.Pools
{
    public interface IPool<T> where T : IPoolable
    {
        T Spawn();
        void Despawn(T obj);
    }
}