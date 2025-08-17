using ElevatorSystem.Domain.Entities;
using System.Threading.Tasks;

namespace ElevatorSystem.Domain.Services
{
    /// <summary>
    /// Handles elevator movement and processes elevator requests.
    /// </summary>
    public class ElevatorMovementService : IElevatorMovementService
    {
        /// <inheritdoc/>
        public async Task ProcessRequest(Elevator elevator, ElevatorRequest request)
        {
            if (elevator.CurrentFloor != request.RequestedFloor)
            {
                await elevator.MoveToFloor(request.RequestedFloor).ConfigureAwait(false);
            }
            elevator.Stop();
        }
    }
}