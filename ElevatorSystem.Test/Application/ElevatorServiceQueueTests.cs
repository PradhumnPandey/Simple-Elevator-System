using ElevatorSystem.Application.Services;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using Moq;
using System.Reflection;
using System.Collections.Concurrent;
using ElevatorSystem.Domain.Entities;

namespace ElevatorSystem.Test.Application
{
    public class ElevatorServiceQueueTests
    {
        [Fact]
        public void RequestElevator_QueuesWhenUnavailable()
        {
            var mockCoord = new Mock<IElevatorCoordinatorService>();
            var mockRepo = new Mock<IElevatorRepository>();
            mockRepo.Setup(r => r.IsAnyElevatorAvailable()).Returns(false);
            var mockLogger = new Mock<ILoggingService>();
            var service = new ElevatorService(mockCoord.Object, mockRepo.Object, mockLogger.Object);

            service.RequestElevator(5);

            // Use reflection to access the private queue for testability
            var queueField = typeof(ElevatorService).GetField("_requestQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(queueField);
            var queue = queueField.GetValue(service) as ConcurrentQueue<ElevatorRequest>;
            Assert.NotNull(queue);
            Assert.False(queue.IsEmpty);
        }

        [Fact]
        public void RequestElevator_LogsErrorOnException()
        {
            var mockCoord = new Mock<IElevatorCoordinatorService>();
            var mockRepo = new Mock<IElevatorRepository>();
            mockRepo.Setup(r => r.IsAnyElevatorAvailable()).Throws(new Exception("fail"));
            var mockLogger = new Mock<ILoggingService>();
            var service = new ElevatorService(mockCoord.Object, mockRepo.Object, mockLogger.Object);

            Assert.Throws<Exception>(() => service.RequestElevator(1));
            mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}