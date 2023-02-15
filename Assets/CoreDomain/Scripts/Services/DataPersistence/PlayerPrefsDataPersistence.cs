using System;
using Handlers.Serializers.Serializer;
using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.Scripts.Services.DataPersistence
{
    public class PlayerPrefsDataPersistence : IDataPersistence
    {
        private readonly ISerializerService _serializer;

        public PlayerPrefsDataPersistence(ISerializerService serializer)
        {
            _serializer = serializer;
        }

        public void Save<T>(string id, T data)
        {
            try
            {
                var dataText = _serializer.SerializeJson(data);
                PlayerPrefs.SetString(id, dataText);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                LogService.LogError($"tried to save {id}, but exception thrown: {e}");
            }
        }

        public T Load<T>(string id, T defaultValue = default)
        {
            try
            {
                if (!PlayerPrefs.HasKey(id))
                {
                    return defaultValue;
                }

                var dataText = PlayerPrefs.GetString(id);
                var data = _serializer.DeserializeJson<T>(dataText);
                return data;
            }
            catch (Exception e)
            {
                LogService.LogError($"tried to load {id}, but exception thrown: {e}");
                throw;
            }
        }
    }
}