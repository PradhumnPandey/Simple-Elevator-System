using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;

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
            configuration.GetSection("ElevatorSystem").Bind(elevatorConfig);

            var serviceProvider = Startup.ConfigureServices(elevatorConfig);
            var elevatorService = serviceProvider.GetService(typeof(IElevatorService)) as IElevatorService ?? default!;
            var elevatorRepository = serviceProvider.GetService(typeof(IElevatorRepository)) as IElevatorRepository ?? default!;
            var logger = serviceProvider.GetService(typeof(LoggingService)) as LoggingService ?? default!;
            var config = serviceProvider.GetService(typeof(ElevatorSystemConfig)) as ElevatorSystemConfig ?? default!;

            // Use the new ConsoleUI for SRP
            var ui = new ConsoleUI(elevatorRepository, elevatorService, logger, config);
            ui.ShowMenu();

            // Start the in-place updating grid display
            var display = new ElevatorGridDisplay(() => elevatorRepository.GetAllElevators());
            display.Start();
        }
    }
}