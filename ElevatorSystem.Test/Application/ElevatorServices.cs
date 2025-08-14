using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Application.Services;
using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.DataAccess;
using ElevatorSystem.Infrastructure.Interfaces;
using Moq;

namespace ElevatorSystem.Tests.ApplicationTests
{
    public class ElevatorServiceTests
    {
        private readonly IElevatorService _elevatorService;
        private readonly Mock<IElevatorCoordinatorService> _mockCoordinatorService;
        private readonly Mock<IElevatorRepository> _mockRepository;
        private readonly ElevatorRepository _repository;
        public ElevatorServiceTests()
        {
            _mockCoordinatorService = new Mock<IElevatorCoordinatorService>();
            _mockRepository = new Mock<IElevatorRepository>();
            var testElevators = new List<Elevator>
                {
                    new Elevator(1),
                    new Elevator(2),
                    new Elevator(3),
                    new Elevator(4),
                    new Elevator(5)
                };
            _repository = new ElevatorRepository(testElevators);
            var mockLogger = new Mock<ElevatorSystem.Infrastructure.Logging.LoggingService>();

            _elevatorService = new ElevatorService( _mockCoordinatorService.Object, _mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void RequestElevator_ShouldCallProcessRequest()
        {
            // Arrange
            int requestedFloor = 3;
            _mockRepository.Setup(s => s.AreElevatorsAvailable()).Returns(true);

            // Act
            _elevatorService.RequestElevator(requestedFloor);

            // Assert
            _mockCoordinatorService.Verify(s => s.ProcessRequest(It.Is<ElevatorRequest>(r => r.RequestedFloor == requestedFloor)), Times.Once);
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