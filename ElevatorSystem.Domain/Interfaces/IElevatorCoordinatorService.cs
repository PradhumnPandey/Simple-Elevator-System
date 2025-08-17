using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Domain.Services
{
    /// <summary>
    /// Defines the contract for coordinating elevator requests within the system.
    /// </summary>
    public interface IElevatorCoordinatorService
    {
        /// <summary>
        /// Processes an elevator request, assigning it to an appropriate elevator.
        /// </summary>
        /// <param name="request">The elevator request to process.</param>
        void ProcessRequest(ElevatorRequest request);
    }
}