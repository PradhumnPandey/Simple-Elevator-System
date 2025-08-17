using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Domain.Enums;
using Xunit;

namespace ElevatorSystem.Test.Domain.Entities
{
    public class ElevatorTests
    {
        [Fact]
        public void AddRequest_AddsToQueue()
        {
            var elevator = new Elevator(1, new ElevatorSettings());
            var request = new ElevatorRequest(2);
            elevator.AddRequest(request);
            // Use reflection or test Step/MoveToFloor to verify request is processed
        }

        [Fact]
        public async Task Step_ProcessesRequests()
        {
            var elevator = new Elevator(1, new ElevatorSettings { MoveDelaySeconds = 0, PickupDropoffDelaySeconds = 0 });
            elevator.AddRequest(new ElevatorRequest(2));
            await elevator.Step();
            Assert.Equal(2, elevator.CurrentFloor);
        }

        [Fact]
        public async Task MoveToFloor_UpdatesCurrentFloor()
        {
            var elevator = new Elevator(1, new ElevatorSettings { MoveDelaySeconds = 0, PickupDropoffDelaySeconds = 0 });
            await elevator.MoveToFloor(3);
            Assert.Equal(3, elevator.CurrentFloor);
        }

        [Fact]
        public void Stop_SetsStatusStopped()
        {
            var elevator = new Elevator(1, new ElevatorSettings());
            elevator.Stop();
            Assert.Equal(ElevatorStatus.Stopped, elevator.Status);
        }
    }
}