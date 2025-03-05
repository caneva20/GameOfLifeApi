using System.Diagnostics;
using System.Text;
using GameOfLife.Models;
using GameOfLife.Services;
using Spectre.Console;

var gradient = new[] {
    "grey11",
    "grey23",
    "grey35",
    "grey46",
    "grey58",
    "grey70",
    "grey82",
    "grey93"
};

var service = new GameService();
var board = new Board {
    LiveCells = BuildCells(150, -20, 20, seed: 1).ToList()
    // LiveCells = new List<BoardCell>() {
    //     new() { X = 0, Y = 0 },
    //     new() { X = 1, Y = 0 },
    //     new() { X = 0, Y = 1 },
    //     new() { X = 1, Y = 1 },
    //
    //     new() { X = 2, Y = 2 },
    //     new() { X = 3, Y = 2 },
    //     new() { X = 2, Y = 3 },
    //     new() { X = 3, Y = 3 },
    // }
};

var state = service.BuildState(board, 1024);

const int boardViewSizeMinX = -40;
const int boardViewSizeMaxX = 40;
const int boardViewSizeMinY = -20;
const int boardViewSizeMaxY = 20;

var boardViewXOffset = 0;
var boardViewYOffset = 0;

AnsiConsole.Live(new Markup("")).Start(context =>
{
    for (var i = 0; i < 1000; i++) {
        UpdateBoardView();
        var boardString = PrintBoard(
            state,
            boardViewSizeMinX + boardViewXOffset,
            boardViewSizeMinY + boardViewYOffset,
            boardViewSizeMaxX + boardViewXOffset,
            boardViewSizeMaxY + boardViewYOffset);

        var watch = Stopwatch.StartNew();
        service.Simulate(state, 1);

        watch.Stop();

        context.UpdateTarget(
            new Markup(
                $"{boardString}\nIteration: {i,4} | Simulation time: {watch.ElapsedTicks / 10000.0:F3} ms | Center: ({boardViewXOffset}, {boardViewYOffset}) | State: {service.ComputeHash(state),12} | Alive: {state.LiveCells.Count:D} | Complete: {service.IsComplete(state)}"));

        context.Refresh();
    }
});

return;

IEnumerable<BoardCell> BuildCells(int quantity, int minCoords, int maxCoords, int seed) {
    var random = new Random(seed);

    for (var i = 0; i < quantity; i++) {
        yield return new BoardCell() { X = random.Next(minCoords, maxCoords), Y = random.Next(minCoords, maxCoords) };
    }
}

(int minX, int minY, int maxX, int maxY) GetBoardDimensions(BoardState state) {
    var minX = state.LiveCells.Min(x => x.x);
    var minY = state.LiveCells.Min(x => x.y);
    var maxX = state.LiveCells.Max(x => x.x);
    var maxY = state.LiveCells.Max(x => x.y);

    return (minX, minY, maxX, maxY);
}

string PrintBoard(BoardState state, int minX, int minY, int maxX, int maxY) {
    var sb = new StringBuilder();

    for (var y = minY - 1; y <= maxY + 1; y++) {
        for (var x = minX - 1; x <= maxX + 1; x++) {
            sb.Append(service.IsAlive(state, (x, y)) ? " [white]■[/]" : " [gray].[/]");
        }

        sb.AppendLine();
    }

    return sb.ToString();
}

void PrintNeighbors(BoardState state) {
    var (minX, minY, maxX, maxY) = GetBoardDimensions(state);

    for (var x = minX - 1; x <= maxX + 1; x++) {
        for (var y = minY - 1; y <= maxY + 1; y++) {
            var neighbourCount = state.Neighbors.GetValueOrDefault((x, y), 0);
            AnsiConsole.Markup($" [{gradient[neighbourCount]}]{neighbourCount}[/] ");
        }

        AnsiConsole.WriteLine();
    }
}

void UpdateBoardView() {
    if (!Console.KeyAvailable) {
        return;
    }

    var key = Console.ReadKey(true).Key;
    switch (key) {
        case ConsoleKey.W:
        case ConsoleKey.UpArrow:
            boardViewYOffset++;
            break;
        case ConsoleKey.S:
        case ConsoleKey.DownArrow:
            boardViewYOffset--;
            break;
        case ConsoleKey.A:
        case ConsoleKey.LeftArrow:
            boardViewXOffset--;
            break;
        case ConsoleKey.D:
        case ConsoleKey.RightArrow:
            boardViewXOffset++;
            break;
        case ConsoleKey.Spacebar:
            boardViewXOffset = 0;
            boardViewYOffset = 0;
            break;
    }
}