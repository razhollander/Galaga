using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
using FSG.iOSKeychain;
#endif

namespace CoreDomain.Scripts.Services.DeviceId
{
    public class DeviceIdService : IDeviceIdService
    {
        private const string KeyName = "DiviceIdentifier";
        
        public string GetId()
        {
            #if UNITY_IOS
            string savedDeviceId = Keychain.GetValue(KeyName);
            if (string.IsNullOrEmpty(savedDeviceId))
            {
                savedDeviceId = SystemInfo.deviceUniqueIdentifier;
                Keychain.SetValue(KeyName, savedDeviceId);
                return savedDeviceId;
            }

            return savedDeviceId;
            
            #endif

            return SystemInfo.deviceUniqueIdentifier;
        }
    }
}