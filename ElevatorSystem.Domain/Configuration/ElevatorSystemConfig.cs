namespace ElevatorSystem.Domain.Configuration
{
    /// <summary>
    /// Represents configuration settings for the elevator system, including the number of elevators and floors.
    /// </summary>
    public class ElevatorSystemConfig
    {
        /// <summary>
        /// Gets or sets the total number of elevators in the system.
        /// </summary>
        public int NumberOfElevators { get; set; }

        /// <summary>
        /// Gets or sets the total number of floors served by the elevator system.
        /// </summary>
        public int NumberOfFloors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorSystemConfig"/> class.
        /// </summary>
        public ElevatorSystemConfig() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorSystemConfig"/> class with the specified number of elevators and floors.
        /// </summary>
        /// <param name="numberOfElevators">The number of elevators in the system.</param>
        /// <param name="numberOfFloors">The number of floors served by the system.</param>
        public ElevatorSystemConfig(int numberOfElevators, int numberOfFloors)
        {
            NumberOfElevators = numberOfElevators;
            NumberOfFloors = numberOfFloors;
        }
    }
}