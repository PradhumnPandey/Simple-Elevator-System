namespace ElevatorSystem.Infrastructure.Logging
{
    public interface ILoggingService
    {
        void LogInfo(string message);
        void LogError(string message, Exception? ex = null);
        void LogSpecial(string message);
    }
}