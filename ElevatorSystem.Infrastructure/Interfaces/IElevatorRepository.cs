namespace ElevatorSystem.Infrastructure.Interfaces
{
    public interface IElevatorRepository
    {
        List<Elevator> GetAllElevators();
        Elevator? GetElevatorById(int id);

        bool AreElevatorsAvailable();
    }
}
