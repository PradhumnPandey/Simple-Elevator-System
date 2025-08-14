using ElevatorSystem.Domain.Enums;

public class ElevatorGridDisplay
{
    private readonly Func<List<Elevator>> _getElevators;
    private readonly int _topRow;
    private const int GridWidth = 40;
    private const int MaxElevators = 10;

    public ElevatorGridDisplay(Func<List<Elevator>> getElevators, int topRow = 0)
    {
        _getElevators = getElevators;
        _topRow = topRow;
    }

    public void Start()
    {
        DrawHeader();

        while (true)
        {
            var elevators = _getElevators();
            int row = _topRow + 2;

            foreach (var elevator in elevators)
            {
                Console.SetCursorPosition(0, row);
                string status = elevator.Status.ToString();
                if (elevator.Status == ElevatorStatus.Moving)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($"| {elevator.Id,3} | {elevator.CurrentFloor,5} | {status,-10} |".PadRight(GridWidth));
                Console.ResetColor();
                row++;
            }

            // Clear any extra lines if elevator count decreases
            for (int i = elevators.Count; i < MaxElevators; i++, row++)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(new string(' ', GridWidth));
            }

            Thread.Sleep(1000);
        }
    }

    private void DrawHeader()
    {
        Console.SetCursorPosition(0, _topRow);
        Console.WriteLine(new string('=', GridWidth));
        Console.SetCursorPosition(0, _topRow + 1);
        Console.WriteLine($"|{"ID",4} |{"Floor",6} |{"Status",-11} |".PadRight(GridWidth));
        Console.SetCursorPosition(0, _topRow + 2);
        Console.WriteLine(new string('-', GridWidth));
    }
}