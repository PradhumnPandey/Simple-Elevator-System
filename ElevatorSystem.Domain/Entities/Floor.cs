namespace ElevatorSystem.Domain.Entities
{
    /// <summary>
    /// Represents a floor within the elevator system.
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// Gets the floor number.
        /// </summary>
        public int FloorNumber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Floor"/> class.
        /// </summary>
        /// <param name="floorNumber">The number of the floor.</param>
        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
        }
    }
}