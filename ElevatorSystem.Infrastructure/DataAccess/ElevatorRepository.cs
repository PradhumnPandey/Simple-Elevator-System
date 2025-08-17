using ElevatorSystem.Domain.Enums;
using ElevatorSystem.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace ElevatorSystem.Infrastructure.DataAccess
{
    /// <summary>
    /// Provides data access and query operations for elevators.
    /// </summary>
    public class ElevatorRepository : IElevatorRepository
    {
        private readonly List<Elevator> _elevators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorRepository"/> class.
        /// </summary>
        /// <param name="elevators">The list of elevators to manage.</param>
        public ElevatorRepository(List<Elevator> elevators)
        {
            _elevators = elevators ?? new List<Elevator>();
        }

        /// <summary>
        /// Determines whether any elevator is available (stopped or ready for next floor).
        /// </summary>
        /// <returns>True if at least one elevator is available; otherwise, false.</returns>
        public bool IsAnyElevatorAvailable()
        {
            for (int i = 0; i < _elevators.Count; i++)
            {
                var status = _elevators[i].Status;
                if (status == ElevatorStatus.Stopped)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all elevators managed by the repository.
        /// </summary>
        /// <returns>The list of elevators.</returns>
        public List<Elevator> GetAllElevators()
        {
            return _elevators;
        }

        /// <summary>
        /// Gets an elevator by its unique identifier.
        /// </summary>
        /// <param name="id">The elevator's unique identifier.</param>
        /// <returns>The elevator if found; otherwise, null.</returns>
        public Elevator? GetElevatorById(int id)
        {
            for (int i = 0; i < _elevators.Count; i++)
            {
                if (_elevators[i].Id == id)
                    return _elevators[i];
            }
            return null;
        }
    }
}