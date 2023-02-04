using System;
using System.Collections.Generic;
using System.IO;
using Core.Services;
using Services.Logs.Base;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Services
{
    public class DebugUIModule
    {
        private List<string> _consoleLogs = new List<string>();
        private List<string> _fullLogs = new List<string>();
        private string _logsLabeID = string.Empty;
        private ILocalStorageService _localStorageService;
        private string _logFileName;

        public DebugUIModule()
        {
            _localStorageService = new LocalStorageService.LocalStorageService();
            Application.logMessageReceived += OnLogRecieved;
            string currentTime = DateTime.Now.ToString("hh_mm_ss");
            _logFileName = "Logs_"+currentTime;
        }

        private void OnShareLogs()
        {
            var shareObj= new NativeShare();
             string filePath = Path.Combine( Application.temporaryCachePath, _logFileName+".txt");
             File.Copy(Path.Combine( Application.persistentDataPath, _logFileName+".logfile"),filePath,true);
             shareObj.AddFile(filePath);
             shareObj.SetSubject("Log file of current run");
             shareObj.SetCallback((result, shareTarget) =>
                 LogService.Log("Share result: " + result + ", selected app: " + shareTarget));
             shareObj.SetCallback((result, shareTarget) =>
                 LogService.Log("Share result: " + result + ", selected app: " + shareTarget));
             shareObj.Share();
        }

        private void saveLogs()
        {
            _localStorageService.WriteBinaryFileToStorage(_logFileName, string.Join('\n', _fullLogs), delegate { },
                delegate(Exception e) { LogService.LogError("Could Not Save Logs"); },
                LocalStorageService.LocalStorageService.FileType.Unique, true, false);
        }
        private void OnLogRecieved(string condition, string stacktrace, LogType type)
        {
            
            string savedLog = $"[{DateTime.Now:h:mm:ss}|{type.ToString()}]: {condition}\n {stacktrace}\n";
            _fullLogs.Add(savedLog);
            if (savedLog.Length > 5)
            {
                saveLogs();
                _fullLogs.Clear();
            }
            string formattedLog = $"<b>[{DateTime.Now:h:mm:ss}|{type.ToString()}]:</b> {condition}\n {stacktrace}\n";
            formattedLog = TruncateLog(formattedLog, 500);
            if (formattedLog.Contains("Error") || formattedLog.Contains("Exception"))
            {
                formattedLog = formattedLog.Replace("<b>", "<color=\"red\"><b>");
                formattedLog = formattedLog.Replace("</b>", "</b></color>");
            }
            _consoleLogs.Insert(0,formattedLog);
            if (_consoleLogs.Count>20)
            {
                _consoleLogs.RemoveAt(_consoleLogs.Count-1);
            }

            var finalLogs = string.Join('\n', _consoleLogs);
        }

        private string TruncateLog( string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}