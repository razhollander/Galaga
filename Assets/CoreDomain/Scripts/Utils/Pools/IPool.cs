namespace CoreDomain.Utils.Pools
{
    public interface IPool<T> where T : IPoolable
    {
        void InitPool();
        T Spawn();
    }
}