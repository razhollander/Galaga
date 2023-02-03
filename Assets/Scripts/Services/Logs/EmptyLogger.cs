using System;
using Services.Logs.Base;

namespace Services.Logs
{
    public class EmptyLogger : LoggerBase
    {
        public override void Log(string message)
        {
        }

        public override void LogWarning(string message)
        {
        }

        public override void LogError(string message)
        {
        }

        public override void LogException(Exception exception)
        {
        }

        public override void LogTag(string message, LogTagType debugLogTag = LogTagType.Temp, string callerFilePath = "", string callerMemberName ="")
        {
        }
    }
}