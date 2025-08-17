using ElevatorSystem.Domain.Enums;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides a grid-based console display for elevator status visualization.
/// </summary>
public class ElevatorGridDisplay
{
    private readonly Func<List<Elevator>> _getElevators;
    private readonly int _topRow;
    private const int GridWidth = 40;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElevatorGridDisplay"/> class.
    /// </summary>
    /// <param name="getElevators">A function to retrieve the current list of elevators.</param>
    /// <param name="topRow">The top row in the console to start drawing the grid.</param>
    public ElevatorGridDisplay(Func<List<Elevator>> getElevators, int topRow = 0)
    {
        _getElevators = getElevators;
        _topRow = topRow;
        Console.Clear();
    }

    /// <summary>
    /// Draws the header of the elevator grid.
    /// </summary>
    public void DrawHeader()
    {
        Console.SetCursorPosition(0, _topRow);
        Console.WriteLine(new string('=', GridWidth));
        Console.SetCursorPosition(0, _topRow + 1);
        Console.WriteLine($"|{"ID",4} |{"Floor",6} |{"Status",-18} |{"Dir",5} |".PadRight(GridWidth));
        Console.SetCursorPosition(0, _topRow + 2);
        Console.WriteLine(new string('-', GridWidth));
    }

    /// <summary>
    /// Renders the elevator grid, displaying each elevator's status.
    /// </summary>
    public void Render()
    {
        var elevators = _getElevators();
        int row = _topRow + 3;

        foreach (var elevator in elevators)
        {
            Console.SetCursorPosition(0, row);
            string status = elevator.Status.ToString();
            string direction = elevator.CurrentDirection.ToString();

            Console.ForegroundColor = elevator.Status == ElevatorStatus.Moving
                ? ConsoleColor.Yellow
                : ConsoleColor.Green;

            Console.Write($"| {elevator.Id,3} | {elevator.CurrentFloor,5} | {status,-17} | {direction,4} |".PadRight(GridWidth));
            Console.ResetColor();
            row++;
        }

        // Position cursor after the last elevator row for further UI output
        Console.SetCursorPosition(0, row + 1);
    }

    /// <summary>
    /// Gets the row number immediately after the last elevator row.
    /// </summary>
    /// <returns>The bottom row index after the grid.</returns>
    public int GetBottomRow()
    {
        var elevators = _getElevators();
        return _topRow + 3 + elevators.Count + 1;
    }
}