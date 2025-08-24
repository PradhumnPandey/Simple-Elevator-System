using System;

namespace ElevatorSystem.Infrastructure.Logging
{
    /// <summary>
    /// Provides thread-safe logging functionality for the elevator system.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private static readonly object _lock = new();
        private readonly string _logFilePath;
        private readonly bool _logToConsole;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// The log file is created in the application's base directory.
        /// </summary>
        /// <param name="logToConsole">Indicates whether to log messages to the console.</param>
        public LoggingService(bool logToConsole = true)
        {
            _logFilePath = Path.Combine(AppContext.BaseDirectory, "elevator_system.log");
            _logToConsole = logToConsole;
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            string logEntry = $"INFO: {message}";
            if (_logToConsole)
            {
                WriteColoredLine($"[INFO] {message}", ConsoleColor.Gray);
            }
            WriteToFile(logEntry);
        }

        /// <summary>
        /// Logs an error message with exception details.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="ex">The exception to log.</param>
        public void LogError(string message, Exception ex)
        {
            string logEntry = $"ERROR: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}" + (ex != null ? $" - Exception: {ex}" : "");
            if (_logToConsole)
            {
                WriteColoredLine($"[ERROR] {message}", ConsoleColor.Red);
                if (ex != null)
                    Console.WriteLine(ex);
            }
            WriteToFile(logEntry);
        }

        /// <summary>
        /// Logs a special message.
        /// </summary>
        /// <param name="message">The special message to log.</param>
        public void LogSpecial(string message)
        {
            if (_logToConsole)
            {
                WriteColoredLine($"[EVENT] {message}", ConsoleColor.Cyan);
                WriteToConsole(message);
            }
            WriteToFile(message);
        }

        /// <summary>
        /// Writes a log entry to the log file in a thread-safe manner.
        /// </summary>
        /// <param name="logEntry">The log entry to write.</param>
        private void WriteToFile(string logEntry)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to write to log file. Please check file permissions or disk space.", ex);
                }
            }
        }

        /// <summary>
        /// Writes a log entry to the console.
        /// </summary>
        /// <param name="logEntry">The log entry to write.</param>
        private void WriteToConsole(string logEntry)
        {
            Console.WriteLine(logEntry);
        }

        /// <summary>
        /// Writes a colored line to the console and resets the color.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="color">The color to use.</param>
        private static void WriteColoredLine(string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }
}