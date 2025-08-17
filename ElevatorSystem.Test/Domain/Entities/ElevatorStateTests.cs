using ElevatorSystem.Domain.States;
using ElevatorSystem.Domain.Configuration;
using Xunit;
using System.Threading.Tasks;

namespace ElevatorSystem.Test.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for elevator state transitions and movement logic.
    /// </summary>
    public class ElevatorStateTests
    {
        [Fact]
        public async Task Elevator_Moves_To_TargetFloor_And_Becomes_Idle()
        {
            // Arrange
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevator = new Elevator(1, settings);
            SetCurrentFloor(elevator, 0);
            elevator.TargetFloor = 3;
            elevator.SetState(new MovingState());

            // Act
            elevator.Handle();
            await Task.Delay(5000);

            // Assert
            Assert.Equal(3, elevator.CurrentFloor);
            Assert.IsType<IdleState>(elevator.State);
        }

        [Fact]
        public void Elevator_Stays_Idle_If_No_TargetFloor()
        {
            // Arrange
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevator = new Elevator(1, settings);
            SetCurrentFloor(elevator, 0);
            elevator.SetState(new IdleState());

            // Act
            elevator.Handle();

            // Assert
            Assert.IsType<IdleState>(elevator.State);
            Assert.Null(elevator.TargetFloor);
        }

        /// <summary>
        /// Sets the current floor of the elevator using reflection.
        /// </summary>
        /// <param name="elevator">The elevator instance.</param>
        /// <param name="floor">The floor to set.</param>
        private static void SetCurrentFloor(Elevator elevator, int floor)
        {
            typeof(Elevator)
                .GetProperty(nameof(Elevator.CurrentFloor))!
                .SetValue(elevator, floor);
        }
    }
}