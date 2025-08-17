namespace ElevatorSystem.Domain.Enums
{
    /// <summary>
    /// Represents the operational status of an elevator.
    /// </summary>
    public enum ElevatorStatus
    {
        /// <summary>
        /// The elevator is currently moving between floors.
        /// </summary>
        Moving,
        /// <summary>
        /// The elevator is picking up or dropping off passengers.
        /// </summary>
        Dropping_Picking,
        /// <summary>
        /// The elevator is stopped and idle.
        /// </summary>
        Stopped
    }
}