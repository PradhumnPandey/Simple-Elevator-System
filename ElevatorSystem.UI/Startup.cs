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
    public static class Startup
    {
        public static ServiceProvider ConfigureServices(ElevatorSystemConfig elevatorConfig)
        {
            return new ServiceCollection()
                .AddSingleton(elevatorConfig)
                .AddSingleton<List<Elevator>>(provider =>
                {
                    var config = provider.GetRequiredService<ElevatorSystemConfig>();
                    var elevators = new List<Elevator>();
                    for (int i = 0; i < config.NumberOfElevators; i++)
                    {
                        elevators.Add(new Elevator(i));
                    }
                    return elevators;
                })
                .AddSingleton<IElevatorCoordinatorService, ElevatorCoordinatorService>(provider =>
                {
                    var config = provider.GetRequiredService<ElevatorSystemConfig>();
                    return new ElevatorCoordinatorService(
                        provider.GetRequiredService<List<Elevator>>(),
                        config.NumberOfFloors
                    );
                })
                .AddSingleton<IElevatorService, ElevatorService>()
                .AddSingleton<IElevatorRepository,ElevatorRepository>()
                .AddSingleton<LoggingService>()
                .BuildServiceProvider();
        }
    }
}