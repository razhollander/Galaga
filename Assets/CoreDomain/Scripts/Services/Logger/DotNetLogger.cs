using System;
using Services.Logs.Base;

namespace Services.Logs
{
    public class DotNetLogger : LoggerBase
    {
        private const string DebugSuffix = "::";

        public override void Log(string message)
        {
            Console.WriteLine(message);
        }

        public override void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public override void LogError(string message)
        {
            throw new Exception(message);
        }

        public override void LogException(Exception exception)
        {
            throw new NotImplementedException(exception.Message);
        }

        public override void LogTag(string message, LogTagType debugLogTagType = LogTagType.Temp, string callerFilePath = "", string callerMemberName ="")
        {
            Console.WriteLine(debugLogTagType + DebugSuffix + message);
        }
    }
}