using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Application.Services;
using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.DataAccess;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ElevatorSystem.Test.Application
{
    /// <summary>
    /// Contains unit tests for the ElevatorService and related repository logic.
    /// </summary>
    public class ElevatorServiceTests
    {
        private readonly IElevatorService _elevatorService;
        private readonly Mock<IElevatorCoordinatorService> _mockCoordinatorService;
        private readonly Mock<IElevatorRepository> _mockRepository;
        private readonly ElevatorRepository _repository;

        public ElevatorServiceTests()
        {
            _mockCoordinatorService = new Mock<IElevatorCoordinatorService>(MockBehavior.Strict);
            _mockRepository = new Mock<IElevatorRepository>(MockBehavior.Strict);

            var testSettings = new ElevatorSettings
            {
                MoveDelaySeconds = 1,
                PickupDropoffDelaySeconds = 1
            };

            var testElevators = new List<Elevator>
            {
                new Elevator(1, testSettings),
                new Elevator(2, testSettings),
                new Elevator(3, testSettings),
                new Elevator(4, testSettings),
                new Elevator(5, testSettings)
            };
            _repository = new ElevatorRepository(testElevators);

            var mockLogger = new Mock<ILoggingService>(); ;

            _elevatorService = new ElevatorService(_mockCoordinatorService.Object, _mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void RequestElevator_ShouldCallProcessRequest()
        {
            // Arrange
            int requestedFloor = 3;
            _mockRepository.Setup(s => s.IsAnyElevatorAvailable()).Returns(true);
            _mockCoordinatorService.Setup(s => s.ProcessRequest(It.Is<ElevatorRequest>(r => r.RequestedFloor == requestedFloor)));

            // Act
            _elevatorService.RequestElevator(requestedFloor);

            // Assert
            _mockCoordinatorService.Verify(s => s.ProcessRequest(It.Is<ElevatorRequest>(r => r.RequestedFloor == requestedFloor)), Times.Once);
            _mockRepository.Verify(s => s.IsAnyElevatorAvailable(), Times.Once);
        }

        [Fact]
        public void GetElevatorStatus_ShouldReturnCorrectElevator()
        {
            // Arrange
            int elevatorId = 1;

            // Act
            var elevator = _repository.GetElevatorById(elevatorId);

            // Assert
            Assert.NotNull(elevator);
            Assert.Equal(elevatorId, elevator.Id);
        }
    }
}