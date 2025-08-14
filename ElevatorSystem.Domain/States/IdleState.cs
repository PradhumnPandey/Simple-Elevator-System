namespace ElevatorSystem.Domain.States
{
    public class IdleState : IElevatorState
    {
        public void Handle(Elevator elevator)
        {
            elevator.Stop();
            if (elevator.TargetFloor.HasValue && elevator.CurrentFloor != elevator.TargetFloor.Value)
            {
                elevator.SetState(new MovingState());
                elevator.Handle();
            }
        }

        public string Status => "Idle";
    }
}