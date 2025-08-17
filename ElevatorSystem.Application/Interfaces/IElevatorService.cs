namespace ElevatorSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for elevator request services.
    /// </summary>
    public interface IElevatorService
    {
        /// <summary>
        /// Requests an elevator to the specified floor.
        /// </summary>
        /// <param name="requestedFloor">The floor number where the elevator is needed.</param>
        void RequestElevator(int requestedFloor);
    }
}