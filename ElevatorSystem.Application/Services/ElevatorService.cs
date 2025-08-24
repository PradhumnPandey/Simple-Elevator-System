using ElevatorSystem.Application.Interfaces;
using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Services;
using ElevatorSystem.Infrastructure.Interfaces;
using ElevatorSystem.Infrastructure.Logging;
using System.Collections.Concurrent;
using ElevatorSystem.Domain.Enums;

namespace ElevatorSystem.Application.Services
{
    /// <summary>
    /// Provides elevator request handling, queuing, and processing logic for the elevator system.
    /// Manages incoming requests, delegates processing to the coordinator, and handles queued requests when elevators become available.
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private readonly IElevatorCoordinatorService _coordinatorService;
        private readonly IElevatorRepository _repository;
        private readonly ILoggingService _logger;
        private readonly ConcurrentQueue<ElevatorRequest> _requestQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _queueProcessingTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorService"/> class.
        /// </summary>
        /// <param name="coordinatorService">Service responsible for coordinating elevator requests.</param>
        /// <param name="repository">Repository for accessing elevator data.</param>
        /// <param name="logger">Logging service for recording events and errors.</param>
        public ElevatorService(
            IElevatorCoordinatorService coordinatorService,
            IElevatorRepository repository,
            ILoggingService logger)
        {
            _coordinatorService = coordinatorService;
            _logger = logger;
            _repository = repository;
            _requestQueue = new ConcurrentQueue<ElevatorRequest>();
            _cancellationTokenSource = new CancellationTokenSource();

            _queueProcessingTask = Task.Run(() => ProcessQueueAutomatically(_cancellationTokenSource.Token));
        }

        /// <summary>
        /// Requests an elevator to the specified floor.
        /// If an elevator is available, the request is processed immediately; otherwise, it is queued.
        /// </summary>
        /// <param name="requestedFloor">The floor number where the elevator is requested.</param>
        public void RequestElevator(int requestedFloor)
        {
            RequestElevator(requestedFloor, "Up");
        }

        /// <summary>
        /// Requests an elevator to the specified floor with direction.
        /// </summary>
        /// <param name="requestedFloor">The floor number where the elevator is requested.</param>
        /// <param name="direction">The direction of the request ("Up" or "Down").</param>
        public void RequestElevator(int requestedFloor, string direction)
        {
            try
            {
                var request = new ElevatorRequest(requestedFloor);
                if (_repository.IsAnyElevatorAvailable())
                {
                    // Find the elevator that will be assigned
                    var elevators = _repository.GetAllElevators();
                    var availableElevators = elevators.Where(e => e.Status == ElevatorStatus.Stopped).ToList();
                    Elevator? assignedElevator = null;
                    if (availableElevators.Any())
                    {
                        assignedElevator = availableElevators.OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor)).First();
                    }
                    else
                    {
                        assignedElevator = elevators.OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor)).First();
                    }

                    _logger.LogInfo($"Elevator {assignedElevator?.Id} assigned to floor {requestedFloor}.");

                    _coordinatorService.ProcessRequest(request);

                    LogRequestProcessing(requestedFloor, assignedElevator?.Id ?? -1);

                    LogRequestCompleted(requestedFloor, assignedElevator?.Id ?? -1);
                    _logger.LogInfo($"Request processed for floor {requestedFloor} by elevator {assignedElevator?.Id}.");
                }
                else
                {
                    _requestQueue.Enqueue(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process request for floor {requestedFloor}.", ex);
                throw;
            }
        }

        public void LogRequestProcessing(int requestedFloor, int elevatorId)
        {
            _logger.LogSpecial($"request for floor {requestedFloor} is being processed by elevator {elevatorId}");
        }

        public void LogRequestCompleted(int requestedFloor, int elevatorId)
        {
            _logger.LogSpecial($"request for floor {requestedFloor} is completed by elevator {elevatorId}");
        }

        /// <summary>
        /// Continuously processes queued elevator requests when elevators become available.
        /// </summary>
        /// <param name="cancellationToken">Token to signal cancellation of the processing loop.</param>
        private async Task ProcessQueueAutomatically(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_requestQueue.IsEmpty && _repository.IsAnyElevatorAvailable())
                {
                    if (_requestQueue.TryDequeue(out var request))
                    {
                        _coordinatorService.ProcessRequest(request);
                        _logger.LogInfo($"Queued request processed for floor {request.RequestedFloor}.");
                    }
                }

                try
                {
                    await Task.Delay(1000, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Stops the background processing of queued elevator requests.
        /// </summary>
        public void StopProcessing()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                _queueProcessingTask.Wait();
            }
            catch (AggregateException ex) when (ex.InnerExceptions.All(e => e is TaskCanceledException))
            {
            }
        }
    }
}