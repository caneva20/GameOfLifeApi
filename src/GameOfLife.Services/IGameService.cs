using GameOfLife.Models;

namespace GameOfLife.Services;

/// <summary>
/// A service that provides functionality for the Game of Life
/// </summary>
public interface IGameService {
    /// <summary>
    /// Builds the initial state of the game from a board
    /// </summary>
    /// <param name="board">The board to build the state from</param>
    /// <param name="maxStateHistory">The maximum amount of history to keep</param>
    /// <returns>The initial state of the game</returns>
    BoardState BuildState(Board board, int maxStateHistory);

    /// <summary>
    /// Simulates the game for a set amount of iterations
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <param name="iterations">The amount of iterations to simulate</param>
    void Simulate(BoardState state, int iterations);

    /// <summary>
    /// Checks if a cell is alive in the current state
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <param name="cell">The cell to check</param>
    /// <returns>True if the cell is alive, false otherwise</returns>
    bool IsAlive(BoardState state, (int x, int y) cell);

    /// <summary>
    /// Updates the neighbours for all cells in the state
    /// </summary>
    /// <param name="state">The current state of the game</param>
    void UpdateNeighbours(BoardState state);

    /// <summary>
    /// Updates the alive cells in the state
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <param name="newCells">The new alive cells</param>
    void UpdateLiveCells(BoardState state, List<(int x, int y)> newCells);

    /// <summary>
    /// Checks if the game has reached a stable state
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <returns>True if the game has reached a stable state, false otherwise</returns>
    bool IsComplete(BoardState state);

    /// <summary>
    /// Computes a hash for the current state of the game
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <returns>The hash of the current state</returns>
    int ComputeHash(BoardState state);
}