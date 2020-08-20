using System;
using NLog;

namespace LoggerService
{
    public class MockLoggerManager : ILoggerManager
    {
        public void LogDebug(string message)
        {
            Console.WriteLine($"**Debug: {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"**Error: {message}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"**Info: {message}");
        }

        public void LogWarn(string message)
        {
            Console.WriteLine($"**Warn: {message}");
        }
    }
}