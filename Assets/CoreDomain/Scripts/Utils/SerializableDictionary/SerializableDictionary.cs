using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();
        [SerializeField]
        private List<TValue> values = new List<TValue>();
 
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var item in this)
            {
                keys.Add(item.Key);
                values.Add(item.Value);
            }
        }
 
        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}
