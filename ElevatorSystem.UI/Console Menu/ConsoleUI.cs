using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using ElevatorSystem.Domain.Configuration;

namespace ElevatorSystem.UI
{
    public class ConsoleUI
    {
        private readonly IElevatorRepository _elevatorRepository;
        private readonly IElevatorService _elevatorService;
        private readonly LoggingService _logger;
        private readonly ElevatorSystemConfig _config;

        public ConsoleUI(
            IElevatorRepository elevatorRepository,
            IElevatorService elevatorService,
            LoggingService logger,
            ElevatorSystemConfig config)
        {
            _elevatorService = elevatorService;
            _elevatorRepository = elevatorRepository;
            _logger = logger;
            _config = config;
        }

        public void ShowMenu()
        {
            _logger.LogInfo("Application started. Showing main menu.");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"  Elevator System");
                Console.WriteLine($"  Elevators: {_config.NumberOfElevators} | Floors: {_config.NumberOfFloors}");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Request Elevator");
                Console.WriteLine("2. Check Elevator Status");
                Console.WriteLine("3. Show Elevators Live");
                Console.WriteLine("========================================");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        RequestElevator();
                        break;
                    case "2":
                        CheckElevatorStatus();
                        break;
                    case "3":
                        _logger.LogInfo("User exited the menu.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select 1, 2, or 3.");
                        _logger.LogInfo($"Invalid menu option selected: {input}");
                        break;
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        private void RequestElevator()
        {
            Console.Write($"Enter requested floor (1 - {_config.NumberOfFloors}): ");
            if (int.TryParse(Console.ReadLine(), out int requestedFloor))
            {
                if (requestedFloor < 0 || requestedFloor > _config.NumberOfFloors)
                {
                    Console.WriteLine("Invalid floor selected.");
                    _logger.LogInfo($"Invalid floor selected: {requestedFloor}");
                    return;
                }
                _logger.LogInfo($"User requested elevator to floor {requestedFloor}.");
                _elevatorService.RequestElevator(requestedFloor);
                Console.WriteLine($"Elevator requested to floor {requestedFloor}.");
            }
            else
            {
                Console.WriteLine("Invalid floor number.");
                _logger.LogInfo("Invalid floor number entered.");
            }
        }

        private void CheckElevatorStatus()
        {
            Console.Write("Enter elevator ID: ");
            if (int.TryParse(Console.ReadLine(), out int elevatorId))
            {
                var elevator = _elevatorRepository.GetElevatorById(elevatorId);
                if (elevator != null)
                {
                    Console.WriteLine($"Elevator {elevator.Id} is on floor {elevator.CurrentFloor} and is {elevator.Status}.");
                    _logger.LogInfo($"Checked status for elevator {elevator.Id}.");
                }
                else
                {
                    Console.WriteLine("Elevator not found.");
                    _logger.LogInfo($"Elevator not found for ID: {elevatorId}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid elevator ID.");
                _logger.LogInfo("Invalid elevator ID entered.");
            }
        }
    }
}