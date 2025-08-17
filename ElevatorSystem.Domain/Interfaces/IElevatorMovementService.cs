using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Domain.Services
{
    /// <summary>
    /// Defines the contract for handling elevator movement and processing requests.
    /// </summary>
    public interface IElevatorMovementService
    {
        /// <summary>
        /// Processes a request for the specified elevator, moving it as needed to fulfill the request.
        /// </summary>
        /// <param name="elevator">The elevator to process the request for.</param>
        /// <param name="request">The elevator request to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ProcessRequest(Elevator elevator, ElevatorRequest request);
    }
}