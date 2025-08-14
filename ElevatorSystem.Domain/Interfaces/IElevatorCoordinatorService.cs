using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Domain.Services
{
    public interface IElevatorCoordinatorService
    {
        void ProcessRequest(ElevatorRequest request);
    }
}