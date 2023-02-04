using System;

namespace Services.Logs.Base
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
        void LogTag(string message, LogTagType logTagType = global::Services.Logs.Base.LogTagType.Temp, string callerFilePath = "", string callerMemberName = "");
    }
}