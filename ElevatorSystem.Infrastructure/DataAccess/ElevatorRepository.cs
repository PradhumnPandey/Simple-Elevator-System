using ElevatorSystem.Domain.Enums;
using ElevatorSystem.Infrastructure.Interfaces;

namespace ElevatorSystem.Infrastructure.DataAccess
{
    public class ElevatorRepository:IElevatorRepository
    {
        private readonly List<Elevator> _elevators;

        public ElevatorRepository(List<Elevator> elevators)
        {
            _elevators = this._elevators = elevators ?? new List<Elevator>();
        }

        public bool AreElevatorsAvailable()
        {
            foreach (var elevator in _elevators)
            {
                if (elevator.Status == ElevatorStatus.Stopped)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Elevator> GetAllElevators()
        {
            return _elevators;
        }

        public Elevator? GetElevatorById(int id)
        {
            return _elevators.Find(e => e.Id == id);
        }
    }
}