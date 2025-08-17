namespace ElevatorSystem.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the contract for elevator repository operations.
    /// </summary>
    public interface IElevatorRepository
    {
        /// <summary>
        /// Gets all elevators managed by the repository.
        /// </summary>
        /// <returns>The list of elevators.</returns>
        List<Elevator> GetAllElevators();

        /// <summary>
        /// Gets an elevator by its unique identifier.
        /// </summary>
        /// <param name="id">The elevator's unique identifier.</param>
        /// <returns>The elevator if found; otherwise, null.</returns>
        Elevator? GetElevatorById(int id);

        /// <summary>
        /// Determines whether any elevator is available (stopped or ready for next floor).
        /// </summary>
        /// <returns>True if at least one elevator is available; otherwise, false.</returns>
        bool IsAnyElevatorAvailable();
    }
}
