using ElevatorSystem.Domain.Enums;

namespace ElevatorSystem.Domain.Entities
{
    /// <summary>
    /// Represents a request for an elevator to stop at a specific floor.
    /// </summary>
    public class ElevatorRequest : IComparable<ElevatorRequest>
    {
        /// <summary>
        /// Gets the floor number requested.
        /// </summary>
        public int RequestedFloor { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorRequest"/> class.
        /// </summary>
        /// <param name="requestedFloor">The floor number being requested.</param>
        public ElevatorRequest(int requestedFloor)
        {
            RequestedFloor = requestedFloor;
        }

        /// <summary>
        /// Compares this request to another based on the requested floor.
        /// </summary>
        /// <param name="other">The other elevator request to compare to.</param>
        /// <returns>
        /// A value less than zero if this request is for a lower floor, zero if equal, or greater than zero if higher.
        /// </returns>
        public int CompareTo(ElevatorRequest? other)
        {
            if (other is null) return 1;
            return RequestedFloor.CompareTo(other.RequestedFloor);
        }
    }
}