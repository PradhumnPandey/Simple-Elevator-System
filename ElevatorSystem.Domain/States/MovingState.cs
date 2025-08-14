namespace ElevatorSystem.Domain.States
{
    public class MovingState : IElevatorState
    {
        public async void Handle(Elevator elevator)
        {
            if (elevator.TargetFloor.HasValue && elevator.CurrentFloor != elevator.TargetFloor.Value)
            {
                await elevator.MoveToFloor(elevator.TargetFloor.Value);
            }

            // After moving, clear the target and transition to Idle
            elevator.TargetFloor = null;
            elevator.SetState(new IdleState());
        }

        public string Status => "Moving";
    }
}