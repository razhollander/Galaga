using System;
using System.IO;
using CoreDomain.Services;
using UnityEngine;

namespace Services.Logs
{
    public class UnityLogger : LoggerBase
    {
        private const string DebugTagSuffix = "::";
        private const string Dot = ".";
        private const string StampFormat = "[{0}] ";
        private const string TimeStampFormat = "HH:mm:ss:ff";

        public override void Log(string message)
        {
            Debug.Log(GetTimeStamp() + message);
        }

        public override void LogWarning(string message)
        {
            Debug.LogWarning(GetTimeStamp() + message);
        }

        public override void LogError(string message)
        {
            Debug.LogError(GetTimeStamp() + message);
        }

        public override void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }

        public override void LogTag(string message, LogTagType debugLogTag = LogTagType.Temp, string callerFilePath = "", string callerMemberName = "")
        {
            Debug.Log(debugLogTag + DebugTagSuffix + GetTimeStamp() + GetCallerName(callerFilePath) + Dot + callerMemberName + message);
        }

        private string GetTimeStamp()
        {
            var timeStamp = DateTime.Now.ToString(TimeStampFormat);
            return string.Format(StampFormat, timeStamp);
        }

        private string GetCallerName(string callerFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(callerFilePath);
            return string.Format(StampFormat, fileName);
        }
    }
}