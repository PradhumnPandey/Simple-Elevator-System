using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Enums;

namespace ElevatorSystem.Domain.Services
{
    /// <summary>
    /// Coordinates elevator assignment and request processing within the system.
    /// </summary>
    public class ElevatorCoordinatorService : IElevatorCoordinatorService
    {
        private readonly List<Elevator> _elevators;
        private readonly int _maxFloor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorCoordinatorService"/> class.
        /// </summary>
        /// <param name="elevators">The list of elevators managed by the coordinator.</param>
        /// <param name="maxFloor">The highest floor number in the system.</param>
        public ElevatorCoordinatorService(List<Elevator> elevators, int maxFloor)
        {
            _elevators = elevators;
            _maxFloor = maxFloor;
        }

        /// <summary>
        /// Processes an elevator request by assigning it to the most suitable elevator.
        /// </summary>
        /// <param name="request">The elevator request to process.</param>
        public void ProcessRequest(ElevatorRequest request)
        {
            if (request.RequestedFloor < 0 || request.RequestedFloor > _maxFloor)
                return;

            Elevator? selectedElevator = null;
            int minDistance = int.MaxValue;

            // Prefer idle (stopped) elevators closest to the requested floor
            foreach (var elevator in _elevators)
            {
                if (elevator.Status == ElevatorStatus.Stopped)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - request.RequestedFloor);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedElevator = elevator;
                    }
                }
            }

            // If no idle elevator, pick the closest one regardless of status
            if (selectedElevator == null)
            {
                minDistance = int.MaxValue;
                foreach (var elevator in _elevators)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - request.RequestedFloor);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedElevator = elevator;
                    }
                }
            }

            if (selectedElevator == null)
                return;

            // Only add request if not already at the requested floor
            if (selectedElevator.CurrentFloor == request.RequestedFloor)
                return;

            selectedElevator.AddRequest(request);

            // Start processing if not already moving
            if (selectedElevator.Status == ElevatorStatus.Stopped)
            {
                _ = selectedElevator.Step();
            }
        }
    }
}