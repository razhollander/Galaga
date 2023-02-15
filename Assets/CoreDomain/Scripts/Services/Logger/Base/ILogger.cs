using System;

namespace CoreDomain.Services
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
        void LogTag(string message, LogTagType logTagType = global::CoreDomain.Services.LogTagType.Temp, string callerFilePath = "", string callerMemberName = "");
    }
}