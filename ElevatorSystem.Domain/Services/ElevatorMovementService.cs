using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Domain.Services
{
    public class ElevatorMovementService : IElevatorMovementService
    {
        public async Task ProcessRequest(Elevator elevator, ElevatorRequest request)
        {
            if (elevator.CurrentFloor != request.RequestedFloor)
            {
                await elevator.MoveToFloor(request.RequestedFloor);
            }
            elevator.Stop();
        }
    }
}