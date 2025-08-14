using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Enums;

namespace ElevatorSystem.Domain.Services
{
    public class ElevatorCoordinatorService : IElevatorCoordinatorService
    {
        private readonly List<Elevator> _elevators;
        private readonly int _maxFloor;

        public ElevatorCoordinatorService(List<Elevator> elevators, int maxFloor)
        {
            _elevators = elevators;
            _maxFloor = maxFloor;
        }

        public void ProcessRequest(ElevatorRequest request)
        {
            if (request.RequestedFloor < 0 || request.RequestedFloor > _maxFloor)
            {
                return;
            }

            var availableElevators = _elevators
                .Where(e => e.Status == ElevatorStatus.Stopped)
                .ToList();

            Elevator selectedElevator;

            if (availableElevators.Any())
            {
                selectedElevator = availableElevators
                    .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
                    .First();
            }
            else
            {
                selectedElevator = _elevators
                    .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
                    .First();
            }

            if (selectedElevator.CurrentFloor != request.RequestedFloor)
            {
                selectedElevator.TargetFloor = request.RequestedFloor;
                selectedElevator.Handle();
            }
        }
    }
}