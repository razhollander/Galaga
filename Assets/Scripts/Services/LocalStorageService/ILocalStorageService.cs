using System;
using Services.LocalStorageService;
using UnityEngine.Events;

namespace Core.Services
{
    public interface ILocalStorageService
    {
        void LoadGeneralFile(UnityAction onComplete);

        void WriteBinaryFileToStorage(string fileName, string data, UnityAction onComplete,
            UnityAction<Exception> onFail, LocalStorageService.FileType fileType = LocalStorageService.FileType.General,bool append =false,bool binary = true);

        void ReadBinaryFileFromStorage(string fileName, UnityAction<string> onComplete,
            UnityAction<Exception> onFail, LocalStorageService.FileType fileType = LocalStorageService.FileType.General);

        bool DoesFileExists(string fileName, LocalStorageService.FileType fileType = LocalStorageService.FileType.General);

        string GetRawMainLocalData();
    }
}