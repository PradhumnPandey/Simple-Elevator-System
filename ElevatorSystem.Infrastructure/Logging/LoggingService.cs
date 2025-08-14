namespace ElevatorSystem.Infrastructure.Logging
{
    public class LoggingService
    {
        private static readonly object _lock = new();
        private readonly string _logFilePath;

        public LoggingService()
        {
            // Log file will be created in the application's base directory
            _logFilePath = Path.Combine(AppContext.BaseDirectory, "elevator_system.log");
        }

        public void LogInfo(string message)
        {
            string logEntry = $"INFO: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            WriteToFile(logEntry);
        }

        public void LogError(string message, Exception ex)
        {
            string logEntry = $"ERROR: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message} - Exception: {ex}";
            WriteToFile(logEntry);
        }

        private void WriteToFile(string logEntry)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch
                {
                    throw new InvalidOperationException("Failed to write to log file. Please check file permissions or disk space.");
                }
            }
        }
    }
}