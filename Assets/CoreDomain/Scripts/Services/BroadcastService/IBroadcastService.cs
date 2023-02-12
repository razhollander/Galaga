using System;

namespace CoreDomain.Services
{
    public interface IBroadcastService
    {
        void Add<T>(Action<T> receiver);
        void Broadcast(object args);
        void Remove<T>(Action<T> receiver);
    }
}