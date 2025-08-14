using ElevatorSystem.Domain.States;

namespace ElevatorSystem.Test.Domain.States
{
    public class IdleStateTests
    {
        [Fact]
        public void IdleState_Transitions_To_MovingState_When_TargetFloor_Set()
        {
            // Arrange
            var elevator = new Elevator(1);
            SetCurrentFloor(elevator, 0);
            elevator.TargetFloor = 2;
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
            var elevator = new Elevator(1);
            SetCurrentFloor(elevator, 2);
            elevator.TargetFloor = 2;
            elevator.SetState(new IdleState());

            // Act
            elevator.Handle();

            // Assert
            Assert.IsType<IdleState>(elevator.State);
        }

        private static void SetCurrentFloor(Elevator elevator, int floor)
        {
            typeof(Elevator)
                .GetProperty("CurrentFloor")!
                .SetValue(elevator, floor);
        }
    }
}