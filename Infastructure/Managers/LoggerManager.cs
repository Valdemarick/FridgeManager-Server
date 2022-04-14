using Application.Common.Interfaces.Managers;
using NLog;

namespace Infastructure.Managers
{
    public class LoggerManager : ILoggerManager
    {
        private readonly static ILogger _logger = LogManager.GetCurrentClassLogger();

        public LoggerManager() { }

        public void LogDebug(string message) => _logger.Debug(message);

        public void LogError(string message) => _logger.Error(message);

        public void LogInfo(string message) => _logger.Info(message);

        public void LogWarn(string message) => _logger.Warn(message);
    }
}