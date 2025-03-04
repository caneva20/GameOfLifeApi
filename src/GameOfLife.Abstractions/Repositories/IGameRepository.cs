using GameOfLife.Models;

namespace GameOfLife.Repositories;

public interface IGameRepository {
    /// <summary>
    /// Creates a game board
    /// </summary>
    /// <param name="board">The board with initial configuration to be created</param>
    /// <returns>The created board</returns>
    Task<Board> CreateBoard(Board board);
}