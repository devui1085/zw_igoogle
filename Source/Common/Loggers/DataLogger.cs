using System;
using System.Text;
using NLog;

namespace Common.Loggers
{
    public class DataLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private DataLogger() { }

        public static void Info(string message, string controllerName, string actionName)
        {
            logger.Info(FormatLogEntry(message, controllerName, actionName));
        }

        public static void Warn(string message, string controllerName, string actionName)
        {
            logger.Warn(FormatLogEntry(message, controllerName, actionName));
        }

        private static string FormatLogEntry(
                       string message,
                       string controllerName, string actionName)
        {
            StringBuilder logEntry = new StringBuilder();
            logEntry.Append("<LOGENTRY>");
            logEntry.AppendFormat("<TIMESTAMP>{0}</TIMESTAMP>", DateTime.Now);
            logEntry.AppendFormat("<Controller>{0}</Controller>", controllerName);
            logEntry.AppendFormat("<Action>{0}</Action>", actionName);
            logEntry.AppendFormat("<MSG>{0}</MSG></LOGENTRY>", message);
            logEntry.AppendLine();

            return logEntry.ToString();
        }

    }
}
