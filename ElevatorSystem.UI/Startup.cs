using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Application.Services;
using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.DataAccess;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSystem.UI
{
    /// <summary>
    /// Configures dependency injection and service registration for the elevator system UI.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Registers and configures all required services for the elevator system.
        /// </summary>
        /// <param name="elevatorConfig">Elevator system configuration.</param>
        /// <param name="elevatorSettings">Elevator operation settings.</param>
        /// <param name="logToConsole">Indicates whether to log messages to the console (should be false for simulation).</param>
        /// <returns>A built <see cref="ServiceProvider"/> for dependency resolution.</returns>
        public static ServiceProvider ConfigureServices(ElevatorSystemConfig elevatorConfig, ElevatorSettings elevatorSettings, bool logToConsole)
        {
            return new ServiceCollection()
                .AddSingleton(elevatorConfig)
                .AddSingleton(elevatorSettings)
                .AddSingleton<List<Elevator>>(provider =>
                {
                    var config = provider.GetRequiredService<ElevatorSystemConfig>();
                    var settings = provider.GetRequiredService<ElevatorSettings>();
                    var elevators = new List<Elevator>(config.NumberOfElevators);
                    for (int i = 0; i < config.NumberOfElevators; i++)
                    {
                        elevators.Add(new Elevator(i, settings));
                    }
                    return elevators;
                })
                .AddSingleton<IElevatorCoordinatorService>(provider =>
                {
                    var config = provider.GetRequiredService<ElevatorSystemConfig>();
                    return new ElevatorCoordinatorService(
                        provider.GetRequiredService<List<Elevator>>(),
                        config.NumberOfFloors
                    );
                })
                .AddSingleton<IElevatorService, ElevatorService>()
                .AddSingleton<IElevatorRepository, ElevatorRepository>()
                .AddSingleton<ILoggingService, LoggingService>(provider => new LoggingService(logToConsole))
                .BuildServiceProvider();
        }
    }
}