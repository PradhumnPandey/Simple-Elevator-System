namespace ElevatorSystem.Domain.States
{
    /// <summary>
    /// Defines the contract for elevator states, enabling state-specific behavior and status reporting.
    /// </summary>
    public interface IElevatorState
    {
        /// <summary>
        /// Handles the logic for the current state using the provided elevator instance.
        /// </summary>
        /// <param name="elevator">The elevator to apply the state logic to.</param>
        void Handle(Elevator elevator);

        /// <summary>
        /// Gets the status string representing the current state.
        /// </summary>
        string Status { get; }
    }
}