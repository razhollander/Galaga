using Newtonsoft.Json;
using UnityEngine;

namespace Handlers
{
    public static class SaveLocallyHandler
    {
        #region --- Public Methods ---

        public static bool LoadBool(string key)
        {
            return PlayerPrefs.GetInt(key) != 0;
        }

        public static T LoadObject<T>(string key)
        {
            var json = LoadString(key);

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        private static string LoadString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void SaveBool(string key, bool boolean)
        {
            PlayerPrefs.SetInt(key, boolean ? 1 : 0);
        }

        public static void SaveObject<T>(string key, T obj)
        {
            if (obj == null)
            {
                return;
            }

            var json = JsonConvert.SerializeObject(obj);

            if (json == null)
            {
                return;
            }

            SaveString(key, json);
        }

        private static void SaveString(string key, string name)
        {
            PlayerPrefs.SetString(key, name);
        }

        #endregion
    }
}