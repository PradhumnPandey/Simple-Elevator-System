using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Domain.Services
{
    public interface IElevatorMovementService
    {
        Task ProcessRequest(Elevator elevator, ElevatorRequest request);
    }
}