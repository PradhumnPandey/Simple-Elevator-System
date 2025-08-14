using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Domain.States;

namespace ElevatorSystem.Test.Domain.Services
{
    public class ElevatorCoordinatorServiceTests
    {
        private const int MaxFloor = 10;

        [Fact]
        public void ProcessRequest_Assigns_Closest_Elevator()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2)
            };
            SetCurrentFloor(elevators[0], 0);
            SetCurrentFloor(elevators[1], 5);
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(1);

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            Assert.Equal(1, elevators[0].TargetFloor); // Closest elevator should be assigned
        }

        [Fact]
        public void ProcessRequest_DoesNotMove_If_Already_At_Requested_Floor()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2)
            };
            SetCurrentFloor(elevators[0], 3);
            SetCurrentFloor(elevators[1], 5);
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(3);

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            Assert.Null(elevators[0].TargetFloor); // No movement needed
        }

        [Fact]
        public void ProcessRequest_Assigns_Moving_Elevator_When_All_Busy()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2)
            };
            SetCurrentFloor(elevators[0], 0);
            SetCurrentFloor(elevators[1], 5);
            // Simulate both elevators as moving by setting their status
            elevators[0].SetState(new MovingState());
            elevators[1].SetState(new MovingState());
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(4);

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            // The closest elevator (elevators[1]) should be assigned
            Assert.Equal(4, elevators[1].TargetFloor);
        }

        [Fact]
        public async Task ProcessRequest_Transitions_State_Correctly()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1)
            };
            SetCurrentFloor(elevators[0], 0);
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(2);

            // Act
            coordinator.ProcessRequest(request);
            // Wait for elevator to process state transitions
            await Task.Delay(2500);

            // Assert
            Assert.Equal(2, elevators[0].CurrentFloor);
            Assert.IsType<IdleState>(elevators[0].State);
        }

        [Fact]
        public void ProcessRequest_Ignores_Invalid_Floor()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1)
            };
            SetCurrentFloor(elevators[0], 0);
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(-1); // Invalid floor

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            // Should not assign a target floor for invalid request
            Assert.Null(elevators[0].TargetFloor);
        }

        [Fact]
        public void ProcessRequest_Accepts_Valid_Floor()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1)
            };
            SetCurrentFloor(elevators[0], 0);
            int maxFloor = 10;
            var coordinator = new ElevatorCoordinatorService(elevators, maxFloor);
            var request = new ElevatorRequest(5); // Valid floor

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            Assert.Equal(5, elevators[0].TargetFloor);
        }

        // Helper method to set private/protected CurrentFloor property for testing
        private static void SetCurrentFloor(Elevator elevator, int floor)
        {
            typeof(Elevator)
                .GetProperty("CurrentFloor")!
                .SetValue(elevator, floor);
        }
    }
}