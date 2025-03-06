using GameOfLife.Models;

namespace GameOfLife.UseCases;

public interface IGetBoardStateUseCase {
    /// <summary>
    /// Retrieves the state of a board, simulating the game up to the specified number of iterations.
    /// </summary>
    /// <param name="id">The id of the board to retrieve.</param>
    /// <param name="iterations">The number of iterations to simulate the board before returning the state.</param>
    /// <returns>
    /// A tuple containing the board and whether the board is in a complete state.
    /// If the board does not exist, the method returns null.
    /// </returns>
    Task<(Board board, bool complete)?> GetBoardState(long id, int iterations);
}