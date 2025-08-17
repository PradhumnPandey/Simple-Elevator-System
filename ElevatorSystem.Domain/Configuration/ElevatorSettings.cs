namespace ElevatorSystem.Domain.Configuration
{
    /// <summary>
    /// Represents configuration settings for elevator operation timing.
    /// </summary>
    public class ElevatorSettings
    {
        /// <summary>
        /// Gets or sets the delay in seconds for the elevator to move between floors.
        /// </summary>
        public int MoveDelaySeconds { get; set; }

        /// <summary>
        /// Gets or sets the delay in seconds for picking up or dropping off passengers.
        /// </summary>
        public int PickupDropoffDelaySeconds { get; set; }
    }
}