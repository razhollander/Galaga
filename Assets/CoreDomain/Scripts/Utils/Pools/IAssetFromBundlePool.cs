namespace CoreDomain.Utils.Pools
{
    public interface IAssetFromBundlePool<T> : IPool<T> where T : IPoolable
    {
        string AssetName { get; }
    }
}