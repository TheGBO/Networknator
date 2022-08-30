using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Networknator.Utils
{
    public class NetworknatorLogger
    {
        private static Dictionary<LogType, LogMethod> logMethods = new Dictionary<LogType, LogMethod>();
        public delegate void LogMethod(string log);

        public static void StartLogger(LogMethod normalLog, LogMethod errorLog, LogMethod warningLog)
        {
            logMethods.Clear();
            logMethods.Add(LogType.normal, normalLog);
            logMethods.Add(LogType.error, errorLog);
            logMethods.Add(LogType.warning, warningLog);
        }

        public static void StartLogger(LogMethod generalLog)
        {
            logMethods.Clear();
            logMethods.Add(LogType.normal, generalLog);
            logMethods.Add(LogType.error, generalLog);
            logMethods.Add(LogType.warning, generalLog);
        }

        public static void Log(LogType logType, object toLog, [CallerLineNumber] int lineNumber = 0,[CallerMemberName] string caller = null, [CallerFilePath] string callerFile = null)
        {
            if(logMethods.TryGetValue(logType, out LogMethod method))
            {
                method($"[{DateTime.Now}] :: " + toLog.ToString() + $"  {callerFile}:{caller} at line {lineNumber}");
            }
        }

    }

    public enum LogType
    {
        normal,
        error,
        warning
    }
}
