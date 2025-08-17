namespace ElevatorSystem.Domain.Enums
{
    /// <summary>
    /// Specifies the direction of elevator movement.
    /// </summary>
    public enum Direction 
    { 
        /// <summary>
        /// The elevator is moving up.
        /// </summary>
        Up,
        /// <summary>
        /// The elevator is moving down.
        /// </summary>
        Down,
        /// <summary>
        /// The elevator is idle (not moving).
        /// </summary>
        Idle,
    }
}
