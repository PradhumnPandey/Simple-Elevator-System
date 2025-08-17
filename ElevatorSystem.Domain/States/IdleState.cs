namespace ElevatorSystem.Domain.States
{
    /// <summary>
    /// Represents the idle state of an elevator.
    /// </summary>
    public class IdleState : IElevatorState
    {
        /// <summary>
        /// Handles the elevator logic when in the idle state.
        /// If a target floor is set and differs from the current floor, transitions to the moving state.
        /// </summary>
        /// <param name="elevator">The elevator instance.</param>
        public void Handle(Elevator elevator)
        {
            elevator.Stop();
            if (elevator.TargetFloor is int targetFloor && elevator.CurrentFloor != targetFloor)
            {
                elevator.SetState(new MovingState());
                elevator.Handle();
            }
        }

        /// <summary>
        /// Gets the status string representing the idle state.
        /// </summary>
        public string Status => "Stopped";
    }
}