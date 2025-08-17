using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Enums;
using ElevatorSystem.Infrastructure.DataAccess;
using Xunit;
using System.Collections.Generic;

namespace ElevatorSystem.Test.Infrastructure
{
    public class ElevatorRepositoryTests
    {
        [Fact]
        public void GetAllElevators_ReturnsAll()
        {
            var elevators = new List<Elevator> { new Elevator(1, new()), new Elevator(2, new()) };
            var repo = new ElevatorRepository(elevators);
            Assert.Equal(2, repo.GetAllElevators().Count);
        }

        [Fact]
        public void GetElevatorById_ReturnsCorrectElevator()
        {
            var elevator = new Elevator(1, new());
            var repo = new ElevatorRepository(new List<Elevator> { elevator });
            Assert.Equal(elevator, repo.GetElevatorById(1));
            Assert.Null(repo.GetElevatorById(99));
        }

        [Fact]
        public void IsAnyElevatorAvailable_ReturnsTrueIfStopped()
        {
            var elevator = new Elevator(1, new());
            var repo = new ElevatorRepository(new List<Elevator> { elevator });
            Assert.True(repo.IsAnyElevatorAvailable());
        }
    }
}