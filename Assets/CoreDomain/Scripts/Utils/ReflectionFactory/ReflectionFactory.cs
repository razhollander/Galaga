using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Services.Logs.Base;

namespace CoreDomain.Scripts.Utils.ReflectionFactory
{
    public abstract class ReflectionFactory<T>
    {
        private readonly Dictionary<string, Type> _types = new();

        private List<Type> _allTypes = null;

        public List<Type> AllTypes
        {
            get
            {
                if (_allTypes == null)
                {
                    _allTypes = _types.Values.ToList();
                }

                return _allTypes;
            }
        }

        public bool Contains(string type)
        {
            foreach (var storedType in AllTypes)
            {
                if (storedType.Name == type)
                {
                    return true;
                }
            }

            return false;
        }

        public ReflectionFactory()
        {
            foreach (var classType in Assembly.GetAssembly(typeof(T)).GetTypes())
            {
                if (!classType.IsAbstract && (classType.IsSubclassOf(typeof(T)) || classType.GetInterfaces().Contains(typeof(T))))
                {
                    _types.Add(classType.Name, classType);
                }
            }
        }

        public T Create(string type)
        {
            if (!_types.ContainsKey(type))
            {
                LogService.LogError($"Type {type} not found in the factory, returning default");
                return default;
            }

            try
            {
                return CreateInstance(_types[type]);
            }
            catch (Exception e)
            {
                LogService.LogError($"Exception when trying to create type {type}");
                LogService.LogError(e.Message);
                return default;
            }
        }

        protected abstract T CreateInstance(Type classType);
    }
}