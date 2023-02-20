using System;

namespace CoreDomain.Services
{
    public abstract class LoggerBase : ILogger
    {
        protected LoggerBase()
        {
            LogService.InjectLogger(this);
        }

        public abstract void Log(string message);
        public abstract void LogWarning(string message);
        public abstract void LogError(string message);
        public abstract void LogException(Exception exception);
        public abstract void LogTag(string message, LogTagType logTagType = LogTagType.Temp, string callerFilePath = "", string callerFileName = "");
    }
}