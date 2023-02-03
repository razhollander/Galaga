using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Handlers.Serializers.Serializer;
using Newtonsoft.Json.Linq;
using Services.SerializerService;
using UnityEngine;
using UnityEngine.Events;

namespace Services.LocalStorageService
{
    public class LocalStorageService : ILocalStorageService
    {
        private const string BINARY_FILES_EXTENSION = ".something";
        private const string GENERAL_FILE_NAME = "general";
        private readonly string _dataPath;
        private Dictionary<string, JObject> _generalSharedData;
        private bool _shouldSaveGenericFile;

        private readonly object readLock = new();
        private readonly object writeLock = new();
        private readonly ISerializerService _serializerService;

        public enum FileType
        {
            Unique,
            General
        }

        public LocalStorageService()
        {
            _dataPath = Application.persistentDataPath;
            _serializerService = new Handlers.Serializers.Serializer.SerializerService();
        }

        public void LoadGeneralFile(UnityAction onComplete)
        {
            _generalSharedData = new Dictionary<string, JObject>();
            // If the general file already exists load it
            if (DoesFileExists(GENERAL_FILE_NAME, FileType.Unique))
            {
                ReadBinaryFileFromStorage(GENERAL_FILE_NAME, _ => OnGeneralFileLoad(_, onComplete),
                    Logs.Base.LogService.LogException, FileType.Unique);
            }
            // Otherwise initialize dictionary
            else
            {
                onComplete();
            }
        }

        private void OnGeneralFileLoad(string data, UnityAction onComplete)
        {
            _serializerService.PopulateJson(data, _generalSharedData);
            onComplete();
        }

        public void WriteBinaryFileToStorage(string fileName, string data, UnityAction onComplete,
            UnityAction<Exception> onFail,FileType fileType = FileType.General,bool append =false,bool binary = true)
        {
            switch (fileType)
            {
                case FileType.General:
                    _generalSharedData[fileName] = JObject.Parse(data);
                    fileName = ReplaceFileExtension(GENERAL_FILE_NAME);
                    string path = Path.Combine(_dataPath, fileName); // TODO : Jenya it looks like we need to send path inside of WriteUniqueBinaryFile
                    // TODO : Jenya WriteUniqueBinaryFile may not call to onComplete ...
                    WriteUniqueBinaryFile(GENERAL_FILE_NAME, _serializerService.SerializeJson(_generalSharedData),
                        OnGeneralFileSaveCompleted, Logs.Base.LogService.LogException);
                    break;
                case FileType.Unique:
                    WriteUniqueBinaryFile(fileName, data, onComplete, onFail,append,binary);
                    break;
                default:
                    Logs.Base.LogService.LogError("File type is not implemented!");
                    break;
            }
        }

        private void OnGeneralFileSaveCompleted()
        {
            if (_shouldSaveGenericFile)
            {
                WriteUniqueBinaryFile(GENERAL_FILE_NAME, _serializerService.SerializeJson(_generalSharedData),
                    OnGeneralFileSaveCompleted, Logs.Base.LogService.LogException);
                _shouldSaveGenericFile = false;
            }
        }

        public string GetRawMainLocalData()
        {
            if (_generalSharedData == null)
            {
                return string.Empty;
            }

            try
            {
                return _serializerService.SerializeJson(_generalSharedData);
            }
            catch (Exception e)
            {
                Logs.Base.LogService.LogException(e);
                return string.Empty;
            }
        }
        
        public void ReadBinaryFileFromStorage(string fileName, UnityAction<string> onComplete,
            UnityAction<Exception> onFail, FileType fileType = FileType.General)
        {
            switch (fileType)
            {
                case FileType.General:

                    if (!DoesFileExists(fileName, fileType))
                    {
                        onFail(new Exception(
                            "Requested file data doesn't exist in general file. Please check file existence with DoesFileExist function"));
                        return;
                    }

                    onComplete(_generalSharedData[fileName].ToString());
                    break;
                case FileType.Unique:
                    ReadUniqueBinaryFile(fileName, onComplete, onFail);
                    break;
                default:
                    Logs.Base.LogService.LogError("File type is not implemented!");
                    break;
            }
        }


        public bool DoesFileExists(string fileName, FileType fileType = FileType.General)
        {
            switch (fileType)
            {
                case FileType.General:
                    return _generalSharedData.ContainsKey(fileName) &&
                           !string.IsNullOrWhiteSpace(_generalSharedData[fileName].ToString());
                case FileType.Unique:
                    string path = Path.Combine(_dataPath, ReplaceFileExtension(fileName));
                    return File.Exists(path);
                default:
                    Logs.Base.LogService.LogError("File type is not supported!");
                    return false;
            }
        }

        private async void WriteUniqueBinaryFile(string fileName, string data, UnityAction onComplete,
            UnityAction<Exception> onFail,bool append = false,bool binary = true)
        {
            fileName = ReplaceFileExtension(fileName);
            string path = Path.Combine(_dataPath, fileName);
            Logs.Base.LogService.Log(
                $"Saving Path: {Path.GetDirectoryName(path)}\nFile Name: {Path.GetFileName(path)}\n");


            // Start async task to save file with binary format
            Task task = Task.Run(() =>
            {
                lock (writeLock)
                {
                    using FileStream stream = new FileStream(path, append ? FileMode.Append : FileMode.Create);
                    if (binary)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, data);
                        stream.Close();
                    }
                    else
                    {
                      
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        stream.Write(bytes,0,bytes.Length);
                        stream.Close();
                    }
                }
            });
            await task;
            if (task.IsCanceled || task.IsFaulted)
            {
                onFail?.Invoke(task.Exception);
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        private async void ReadUniqueBinaryFile(string fileName, UnityAction<string> onComplete,
            UnityAction<Exception> onFail)
        {
            fileName = ReplaceFileExtension(fileName);
            string path = Path.Combine(this._dataPath, fileName);
            string data = string.Empty;
            
            Task<string> task = Task.Run(() =>
            {
                lock (readLock)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using FileStream stream = new FileStream(path, FileMode.Open);
                    string extractedData = (string)formatter.Deserialize(stream);
                    stream.Close();

                    return extractedData;
                }
            });
            await task;
            if (task.IsCanceled || task.IsFaulted)
            {
                onFail?.Invoke(task.Exception);
            }
            else
            {
                data = task.Result;
            }

            Logs.Base.LogService.Log(
                $"File Loaded - Path: {Path.GetDirectoryName(path)}\nFile Name: {Path.GetFileName(path)}");
            onComplete?.Invoke(data);
        }

        private static string ReplaceFileExtension(string fileName)
        {
            if (Path.HasExtension(fileName))
            {
                string wrongExtension = Path.GetExtension(fileName);
                fileName = fileName.Replace(wrongExtension, BINARY_FILES_EXTENSION);
            }
            else
            {
                fileName += BINARY_FILES_EXTENSION;
            }

            return fileName;
        }
    }
}