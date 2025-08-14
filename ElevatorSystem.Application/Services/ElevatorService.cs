using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;

namespace ElevatorSystem.Application.Services
{
    public class ElevatorService : IElevatorService
    {
        private readonly IElevatorCoordinatorService _coordinatorService;
        private readonly IElevatorRepository _repository;
        private readonly LoggingService _logger;
        private readonly Queue<ElevatorRequest> _requestQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _queueProcessingTask;

        public ElevatorService(
            IElevatorCoordinatorService coordinatorService,
            IElevatorRepository repository,
            LoggingService logger)
        {
            _coordinatorService = coordinatorService;
            _logger = logger;
            _repository = repository;
            _requestQueue = new Queue<ElevatorRequest>();
            _cancellationTokenSource = new CancellationTokenSource();

            _queueProcessingTask = Task.Run(() => ProcessQueueAutomatically(_cancellationTokenSource.Token));
        }

        public void RequestElevator(int requestedFloor)
        {
            _logger.LogInfo($"Request received for floor {requestedFloor}.");
            try
            {
                var request = new ElevatorRequest(requestedFloor);
                if (_repository.AreElevatorsAvailable())
                {
                    _coordinatorService.ProcessRequest(request);
                    _logger.LogInfo($"Request processed for floor {requestedFloor}.");
                }
                else
                {
                    _requestQueue.Enqueue(request);
                    _logger.LogInfo($"All elevators are occupied. Request for floor {requestedFloor} added to the queue.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process request for floor {requestedFloor}.", ex);
                throw;
            }
        }

        private async Task ProcessQueueAutomatically(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_requestQueue.Count > 0 && _repository.AreElevatorsAvailable())
                {
                    var request = _requestQueue.Dequeue();
                    _coordinatorService.ProcessRequest(request);
                    _logger.LogInfo($"Queued request processed for floor {request.RequestedFloor}.");
                }

                await Task.Delay(1000, cancellationToken);
            }
        }

        public void StopProcessing()
        {
            _cancellationTokenSource.Cancel();
            _queueProcessingTask.Wait();
        }
    }
}