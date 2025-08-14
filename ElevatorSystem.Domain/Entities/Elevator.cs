using ElevatorSystem.Domain.Enums;
using ElevatorSystem.Domain.States;

public class Elevator
{
    public int Id { get; private set; }
    public int CurrentFloor { get; private set; }
    public ElevatorStatus Status { get; private set; }
    public int? TargetFloor { get; set; }
    public IElevatorState State { get; private set; }

    public Elevator(int id)
    {
        Id = id;
        CurrentFloor = 0;
        Status = ElevatorStatus.Stopped;
        State = new IdleState();
    }

    public async Task MoveToFloor(int floor)
    {
        Status = ElevatorStatus.Moving;
        while (CurrentFloor != floor)
        {
            if (CurrentFloor < floor)
                CurrentFloor++;
            else
                CurrentFloor--;

            await Task.Delay(1000);
        }
        Status = ElevatorStatus.Stopped;
    }

    public void Stop()
    {
        Status = ElevatorStatus.Stopped;
    }

    public void SetState(IElevatorState state)
    {
        State = state;
    }

    public void Handle()
    {
        State.Handle(this);
    }
}