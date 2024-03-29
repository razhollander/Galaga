using System;
using System.Collections.Generic;

namespace CoreDomain.Services
{
    public class BroadcastService : IBroadcastService
    {
        private readonly Dictionary<Type, List<Delegate>> _typeDelegatesDictionary;
        
        public BroadcastService()
        {
            _typeDelegatesDictionary = new Dictionary<Type, List<Delegate>>();
        }
        
        public void Add<T>(Action<T> receiver)
        {
            var broadcastType = typeof(T);

            if (!_typeDelegatesDictionary.ContainsKey(broadcastType))
            {
                _typeDelegatesDictionary.Add(broadcastType, new List<Delegate>());
            }

            _typeDelegatesDictionary[broadcastType].Add(receiver);
        }

        public void Broadcast(object args)
        {
            var broadcastType = args.GetType();

            if (!_typeDelegatesDictionary.ContainsKey(broadcastType))
            {
                return;
            }

            var delegatesList = _typeDelegatesDictionary[broadcastType];

            foreach (var delegateAction in delegatesList)
            {
                delegateAction.DynamicInvoke(args);
            }
        }

        public void Remove<T>(Action<T> receiver)
        {
            var broadcastType = typeof(T);

            if (!_typeDelegatesDictionary.ContainsKey(broadcastType))
            {
                return;
            }

            var delegatesList = _typeDelegatesDictionary[broadcastType];

            for (var i = delegatesList.Count - 1; i >= 0; i--)
            {
                if (delegatesList[i] == (Delegate) receiver)
                {
                    delegatesList.RemoveAt(i);
                }
            }
        }
    }
}