using GameOfLife.Models;

namespace GameOfLife.UseCases;

public interface IGetBoardStateUseCase {
    /// <summary>
    /// Gets the state of a board after a number of iterations.
    /// </summary>
    /// <param name="id">The id of the board to get the state of.</param>
    /// <param name="iterations">The number of iterations to simulate.</param>
    /// <returns>The state of the board after the specified number of iterations.</returns>
    Task<Board?> GetBoardState(long id, int iterations);
}