using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using ElevatorSystem.Domain.Configuration;

namespace ElevatorSystem.UI
{
    /// <summary>
    /// Provides a console-based entry point for running the elevator system simulation.
    /// </summary>
    public class ConsoleUI
    {
        private readonly IElevatorService _elevatorService;
        private readonly ElevatorSystemConfig _config;

        public ConsoleUI(
            IElevatorService elevatorService,
            ILoggingService logger,
            ElevatorSystemConfig config)
        {
            _elevatorService = elevatorService;
            _config = config;
        }

        public async Task RunSimulationAsync()
        {
            // Create and run the simulation
            var simulation = new Simulation(_elevatorService);
            await simulation.SimulateMultipleRequestsAsync(_config.NumberOfFloors, 50);
        }
    }
}