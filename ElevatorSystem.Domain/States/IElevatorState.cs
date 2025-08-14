namespace ElevatorSystem.Domain.States
{
    public interface IElevatorState
    {
        void Handle(Elevator elevator);
        string Status { get; }
    }
}