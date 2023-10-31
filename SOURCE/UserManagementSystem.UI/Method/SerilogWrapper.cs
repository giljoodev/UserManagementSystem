using Serilog;
using System;

namespace UserManagementSystem.UI.Method
{
    public class SerilogWrapper
    {
        public SerilogWrapper()
        {
        }

        public ILogger GetLogger(string logFileName = "log.log")
        {
            string path = GetLogFilePath();
            
            return new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File($"{path}/{logFileName}", shared: true).CreateLogger();
        }

        private static string GetLogFilePath()
        {
            // Specify the base directory where log files should be stored.
            string logDirectory = "./Logs";

            // Create a subdirectory for the current date.
            string subDirectory = DateTime.Now.ToString("yyyy-MM-dd");

            // Combine the base directory and subdirectory to get the full log file path.
            string logFilePath = System.IO.Path.Combine(logDirectory, subDirectory);

            return logFilePath;
        }
    }
}
