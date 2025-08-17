using ElevatorSystem.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            int requestsSent = 0;
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

            // Request generation loop
            while (requestsSent < totalRequests)
            {
                for (int i = 0; i < 5 && requestsSent < totalRequests; i++, requestsSent++)
                {
                    int floor = _random.Next(1, numberOfFloors + 1);
                    _elevatorService.RequestElevator(floor);
                }
                await Task.Delay(1000).ConfigureAwait(false);
            }

            // Allow UI to update one last time and then stop
            await Task.Delay(1000).ConfigureAwait(false);

            if (uiTask != null)
            {
                try { await uiTask.ConfigureAwait(false); } catch (OperationCanceledException) { }
            }

            cts.Cancel();

            Console.WriteLine("Simulation complete. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
