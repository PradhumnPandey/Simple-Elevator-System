using ElevatorSystem.Domain.States;
using ElevatorSystem.Domain.Configuration;
using Xunit;

namespace ElevatorSystem.Test.Domain.States
{
    /// <summary>
    /// Contains unit tests for IdleState transitions in the elevator state machine.
    /// </summary>
    public class IdleStateTests
    {
        [Fact]
        public void IdleState_Transitions_To_MovingState_When_TargetFloor_Set()
        {
            // Arrange
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevator = new Elevator(1, settings)
            {
                TargetFloor = 2
            };
            SetCurrentFloor(elevator, 0);
            elevator.SetState(new IdleState());

            // Act
            elevator.Handle();

            // Assert
            Assert.IsType<MovingState>(elevator.State);
        }

        [Fact]
        public void IdleState_Does_Not_Transition_If_At_TargetFloor()
        {
            // Arrange
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevator = new Elevator(1, settings)
            {
                TargetFloor = 2
            };
            SetCurrentFloor(elevator, 2);
            elevator.SetState(new IdleState());

            // Act
            elevator.Handle();

            // Assert
            Assert.IsType<IdleState>(elevator.State);
        }

        /// <summary>
        /// Sets the current floor of the elevator using reflection.
        /// </summary>
        private static void SetCurrentFloor(Elevator elevator, int floor)
        {
            typeof(Elevator)
                .GetProperty(nameof(Elevator.CurrentFloor))!
                .SetValue(elevator, floor);
        }
    }
}