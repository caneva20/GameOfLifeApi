using GameOfLife.Models;

namespace GameOfLife.Services;

public sealed class GameService : IGameService {
    private static readonly int[] NeighbourOffsets = { -1, 0, 1 };

    public BoardState BuildState(Board board, int maxStateHistory) {
        var state = new BoardState(maxStateHistory);

        UpdateLiveCells(state, board.LiveCells.Select(x => (x.X, x.Y)).ToList());
        UpdateNeighbours(state);

        return state;
    }

    public void Simulate(BoardState state, int iterations) {
        for (var i = 0; i < iterations; i++) {
            var nextCells = new List<(int x, int y)>();

            foreach (var (cell, neighborCount) in state.Neighbors) {
                var isAlive = IsAlive(state, cell);

                if (isAlive && neighborCount is 2 or 3) {
                    nextCells.Add(cell);
                }

                if (!isAlive && neighborCount is 3) {
                    nextCells.Add(cell);
                }
            }

            UpdateLiveCells(state, nextCells);
            UpdateNeighbours(state);
        }
    }

    public bool IsAlive(BoardState state, (int x, int y) cell) {
        return state.LiveCells.Contains(cell);
    }

    public bool IsComplete(BoardState state) {
        return state.HashHistory.Contains(ComputeHash(state));
    }

    public int ComputeHash(BoardState state) {
        var hash = 17;
        foreach (var cell in state.LiveCells) {
            hash = hash * 31 + cell.x;
            hash = hash * 31 + cell.y;
        }

        return hash;
    }

    public void UpdateLiveCells(BoardState state, List<(int x, int y)> newCells) {
        state.HashHistory.Enqueue(ComputeHash(state));
        if (state.HashHistory.Count > state.MaxStateHistory) {
            state.HashHistory.Dequeue();
        }

        state.LiveCells.Clear();

        foreach (var cell in newCells.OrderBy(x => x.x).ThenBy(x => x.y)) {
            state.LiveCells.Add(cell);
        }
    }

    public void UpdateNeighbours(BoardState state) {
        state.Neighbors.Clear();

        foreach (var cell in state.LiveCells) {
            UpdateNeighbours(state, cell);
        }
    }

    private static void UpdateNeighbours(BoardState state, (int x, int y) cell) {
        foreach (var dx in NeighbourOffsets) {
            foreach (var dy in NeighbourOffsets) {
                if (dx == 0 && dy == 0) {
                    continue;
                }

                var neighbour = (cell.x + dx, cell.y + dy);
                state.Neighbors.TryAdd(neighbour, 0);
                state.Neighbors[neighbour]++;
            }
        }
    }
}