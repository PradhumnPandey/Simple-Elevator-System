using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Domain.States;
using ElevatorSystem.Domain.Configuration;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevatorSystem.Test.Domain.Services
{
    /// <summary>
    /// Contains unit tests for ElevatorCoordinatorService assignment and state logic.
    /// </summary>
    public class ElevatorCoordinatorServiceTests
    {
        private const int MaxFloor = 10;

        [Fact]
        public void ProcessRequest_Assigns_Closest_Elevator()
        {
            // Arrange
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings),
                new Elevator(2, settings)
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
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings),
                new Elevator(2, settings)
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
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings),
                new Elevator(2, settings)
            };
            SetCurrentFloor(elevators[0], 0);
            SetCurrentFloor(elevators[1], 5);
            // Simulate both elevators as moving by setting their state
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
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings)
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
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings)
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
            var settings = new ElevatorSettings { MoveDelaySeconds = 1, PickupDropoffDelaySeconds = 1 };
            var elevators = new List<Elevator>
            {
                new Elevator(1, settings)
            };
            SetCurrentFloor(elevators[0], 0);
            var coordinator = new ElevatorCoordinatorService(elevators, MaxFloor);
            var request = new ElevatorRequest(5); // Valid floor

            // Act
            coordinator.ProcessRequest(request);

            // Assert
            Assert.Equal(5, elevators[0].TargetFloor);
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