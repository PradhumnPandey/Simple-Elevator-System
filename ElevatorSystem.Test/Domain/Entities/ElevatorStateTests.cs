using ElevatorSystem.Domain.States;

namespace ElevatorSystem.Test.Domain.Entities
{
    public class ElevatorStateTests
    {
        [Fact]
        public async Task Elevator_Moves_To_TargetFloor_And_Becomes_Idle()
        {
            // Arrange
            var elevator = new Elevator(1);
            SetCurrentFloor(elevator, 0);
            elevator.TargetFloor = 3;
            elevator.SetState(new MovingState());

            // Act
            elevator.Handle();
            await Task.Delay(3500);

            // Assert
            Assert.Equal(3, elevator.CurrentFloor);
            Assert.IsType<IdleState>(elevator.State);
        }

        [Fact]
        public void Elevator_Stays_Idle_If_No_TargetFloor()
        {
            // Arrange
            var elevator = new Elevator(1);
            SetCurrentFloor(elevator, 0);
            elevator.SetState(new IdleState());

            // Act
            elevator.Handle();

            // Assert
            Assert.IsType<IdleState>(elevator.State);
            Assert.Null(elevator.TargetFloor);
        }

        private void SetCurrentFloor(Elevator elevator, int floor)
        {
            typeof(Elevator)
                .GetProperty("CurrentFloor")!
                .SetValue(elevator, floor);
        }
    }
}