using ElevatorSystem.Domain.Configuration;
using ElevatorSystem.Domain.Entities;
using ElevatorSystem.Domain.Enums;
using ElevatorSystem.Domain.States;

/// <summary>
/// Represents an elevator, managing its state, requests, and movement logic.
/// </summary>
public class Elevator
{
    private readonly ElevatorSettings _settings;

    /// <summary>
    /// Gets the unique identifier for the elevator.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the current floor of the elevator.
    /// </summary>
    public int CurrentFloor { get; private set; }

    /// <summary>
    /// Gets the current status of the elevator.
    /// </summary>
    public ElevatorStatus Status { get; private set; }

    /// <summary>
    /// Gets or sets the target floor for the elevator.
    /// </summary>
    public int? TargetFloor { get; set; }

    /// <summary>
    /// Gets the current state of the elevator.
    /// </summary>
    public IElevatorState State { get; private set; }

    /// <summary>
    /// Gets the current direction of the elevator.
    /// </summary>
    public Direction CurrentDirection { get; private set; } = Direction.Idle;

    // SortedSet ensures requests are ordered by floor; not thread-safe, so external synchronization is required if accessed concurrently.
    private readonly SortedSet<ElevatorRequest> _requests = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Elevator"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the elevator.</param>
    /// <param name="settings">The elevator operation settings.</param>
    public Elevator(int id, ElevatorSettings settings)
    {
        Id = id;
        _settings = settings;
        CurrentFloor = 0;
        Status = ElevatorStatus.Stopped;
        State = new IdleState();
    }

    /// <summary>
    /// Adds a new floor request to the elevator.
    /// </summary>
    /// <param name="elevatorRequest">The elevator request to add.</param>
    public void AddRequest(ElevatorRequest elevatorRequest)
    {
        _requests.Add(elevatorRequest);
        // Set TargetFloor if not already set or if this is the only request
        if (TargetFloor == null || _requests.Count == 1)
        {
            TargetFloor = elevatorRequest.RequestedFloor;
        }
    }

    /// <summary>
    /// Processes all pending requests, moving the elevator as needed.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task Step(CancellationToken cancellationToken = default)
    {
        while (_requests.Count > 0)
        {
            UpdateDirection();

            ElevatorRequest? nextRequest = null;
            if (CurrentDirection == Direction.Up)
            {
                nextRequest = _requests.FirstOrDefault(r => r.RequestedFloor > CurrentFloor);
            }
            else if (CurrentDirection == Direction.Down)
            {
                nextRequest = _requests.Reverse().FirstOrDefault(r => r.RequestedFloor < CurrentFloor);
            }

            if (nextRequest != null)
            {
                await MoveToFloor(nextRequest.RequestedFloor, cancellationToken).ConfigureAwait(false);
                _requests.Remove(nextRequest);
            }
            else
            {
                // No requests in the current direction, update direction and check again
                UpdateDirection();
                if (CurrentDirection == Direction.Idle)
                    break;
            }
        }
        Status = ElevatorStatus.Stopped;
        CurrentDirection = Direction.Idle;
    }

    /// <summary>
    /// Moves the elevator to the specified floor, simulating delays for movement and pickup/dropoff.
    /// </summary>
    /// <param name="floor">The floor to move to.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task MoveToFloor(int floor, CancellationToken cancellationToken = default)
    {
        Status = ElevatorStatus.Moving;
        while (CurrentFloor != floor)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CurrentFloor += (CurrentFloor < floor) ? 1 : -1;
            Console.WriteLine($"[STATUS] Elevator {Id}: Floor {CurrentFloor}"); // Log only on movement
            await Task.Delay(TimeSpan.FromSeconds(_settings.MoveDelaySeconds), cancellationToken).ConfigureAwait(false);
        }
        Status = ElevatorStatus.Dropping_Picking;
        Console.WriteLine($"[INFO] Elevator {Id} arrived at floor {floor}. Passengers entering/leaving...");
        await Task.Delay(TimeSpan.FromSeconds(_settings.PickupDropoffDelaySeconds), cancellationToken).ConfigureAwait(false);
        Status = ElevatorStatus.Stopped;
        CurrentDirection = Direction.Idle;

        _requests.RemoveWhere(r => r.RequestedFloor == floor);
    }

    /// <summary>
    /// Updates the elevator's direction based on pending requests and current position.
    /// </summary>
    private void UpdateDirection()
    {
        if (_requests.Count == 0)
        {
            CurrentDirection = Direction.Idle;
            return;
        }

        if (CurrentDirection == Direction.Idle)
        {
            var firstRequest = _requests.First();
            if (firstRequest.RequestedFloor > CurrentFloor)
                CurrentDirection = Direction.Up;
            else if (firstRequest.RequestedFloor < CurrentFloor)
                CurrentDirection = Direction.Down;
        }
        else
        {
            bool hasRequestsInCurrentDirection = CurrentDirection switch
            {
                Direction.Up => _requests.Any(r => r.RequestedFloor > CurrentFloor),
                Direction.Down => _requests.Any(r => r.RequestedFloor < CurrentFloor),
                _ => false
            };

            if (!hasRequestsInCurrentDirection)
            {
                if (CurrentDirection == Direction.Up && _requests.Any(r => r.RequestedFloor < CurrentFloor))
                    CurrentDirection = Direction.Down;
                else if (CurrentDirection == Direction.Down && _requests.Any(r => r.RequestedFloor > CurrentFloor))
                    CurrentDirection = Direction.Up;
                else
                    CurrentDirection = Direction.Idle;
            }
        }
    }

    /// <summary>
    /// Stops the elevator and sets its status to stopped.
    /// </summary>
    public void Stop()
    {
        Status = ElevatorStatus.Stopped;
    }

    /// <summary>
    /// Sets the elevator's state and updates its status accordingly.
    /// </summary>
    /// <param name="state">The new state to set.</param>
    public void SetState(IElevatorState state)
    {
        State = state;
        if (Enum.TryParse<ElevatorStatus>(state.Status.Replace("/", "_"), out var parsedStatus))
        {
            Status = parsedStatus;
        }
    }

    /// <summary>
    /// Invokes the current state's handle logic for the elevator.
    /// </summary>
    public void Handle()
    {
        State.Handle(this);
    }
}