using ElevatorSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ElevatorSystem.UI
{
    /// <summary>
    /// Simulates multiple elevator requests and optionally updates the console UI in real time.
    /// </summary>
    public class Simulation
    {
        private readonly IElevatorService _elevatorService;
        private readonly ElevatorGridDisplay? _display;
        private readonly Random _random = new();

        public Simulation(IElevatorService elevatorService, ElevatorGridDisplay? display = null)
        {
            _elevatorService = elevatorService;
            _display = display;
        }

        /// <summary>
        /// Simulates multiple elevator requests and optionally updates the display concurrently.
        /// </summary>
        /// <param name="numberOfFloors">The number of floors in the building.</param>
        /// <param name="totalRequests">The total number of requests to simulate.</param>
        public async Task SimulateMultipleRequestsAsync(int numberOfFloors, int totalRequests = 50)
        {
            var requests = new List<int>();
            // 1. Collect all requests first
            for (int i = 0; i < totalRequests; i++)
            {
                int floor = _random.Next(1, numberOfFloors + 1);
                requests.Add(floor);
            }

            // Log all requests in one line with proper grammar
            if (requests.Count == 1)
            {
                Console.WriteLine($"Request for floor {requests[0]} has been received.");
            }
            else if (requests.Count == 2)
            {
                Console.WriteLine($"Requests for floors {requests[0]} and {requests[1]} have been received.");
            }
            else if (requests.Count > 2)
            {
                var allButLast = requests.Take(requests.Count - 1).Select(f => f.ToString());
                var last = requests.Last();
                Console.WriteLine($"Requests for floors {string.Join(", ", allButLast)}, and {last} have been received.");
            }

            // 2. Optionally show UI while waiting (if needed)
            using var cts = new CancellationTokenSource();
            Task? uiTask = null;
            if (_display != null)
            {
                uiTask = Task.Run(() =>
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        _display.DrawHeader();
                        _display.Render();
                        Thread.Sleep(1000);
                    }
                }, cts.Token);
            }

            // 3. Now process all requests at once
            foreach (var floor in requests)
            {
                _elevatorService.RequestElevator(floor);
            }

            // Allow UI to update one last time and then stop
            await Task.Delay(1000).ConfigureAwait(false);

            if (uiTask != null)
            {
                try { await uiTask.ConfigureAwait(false); } catch (OperationCanceledException) { }
            }

            cts.Cancel();
            Console.ReadLine();
        }
    }
}
