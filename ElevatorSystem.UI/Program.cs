using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSystem.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Bind to strongly-typed config
            var elevatorConfig = new ElevatorSystemConfig();
            var elevatorSettings = new ElevatorSettings();
            configuration.GetSection("ElevatorSystem").Bind(elevatorConfig);
            configuration.GetSection("ElevatorSettings").Bind(elevatorSettings);

            // Read GridDisplayEnabled from appsettings.json
            bool displayGrid = configuration.GetValue<bool>("GridDisplayEnabled");

            // Pass !displayGrid to control console logging: only log to console when not simulating
            var serviceProvider = Startup.ConfigureServices(elevatorConfig, elevatorSettings, !displayGrid);
            var elevatorService = serviceProvider.GetRequiredService<IElevatorService>();
            var elevatorRepository = serviceProvider.GetRequiredService<IElevatorRepository>();
            var logger = serviceProvider.GetRequiredService<ILoggingService>();
            var config = serviceProvider.GetRequiredService<ElevatorSystemConfig>();

            if (displayGrid)
            {
                var display = new ElevatorGridDisplay(() => elevatorRepository.GetAllElevators(), topRow: 0);
                var sim = new Simulation(elevatorService, display);
                sim.SimulateMultipleRequestsAsync(config.NumberOfFloors, 50).GetAwaiter().GetResult();
            }
            else
            {
                // Load simple console UI
                var ui = new ConsoleUI(elevatorService, logger, config);
                ui.RunSimulationAsync().GetAwaiter().GetResult();
            }
        }
    }
}