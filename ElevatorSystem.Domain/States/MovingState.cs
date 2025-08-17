namespace ElevatorSystem.Domain.States
{
    /// <summary>
    /// Represents the moving state of the elevator.
    /// </summary>
    public class MovingState : IElevatorState
    {
        /// <summary>
        /// Handles the elevator logic when in the moving state.
        /// Moves the elevator to the target floor asynchronously, then transitions to idle.
        /// </summary>
        /// <param name="elevator">The elevator instance.</param>
        public void Handle(Elevator elevator)
        {
            _ = MoveAndTransitionAsync(elevator);
        }

        private async Task MoveAndTransitionAsync(Elevator elevator)
        {
            if (elevator.TargetFloor is int targetFloor && elevator.CurrentFloor != targetFloor)
            {
                await elevator.MoveToFloor(targetFloor).ConfigureAwait(false);
            }

            // After moving, clear the target and transition to Idle
            elevator.TargetFloor = null;
            elevator.SetState(new IdleState());
        }

        /// <summary>
        /// Gets the status string representing the moving state.
        /// </summary>
        public string Status => "Moving";
    }
}