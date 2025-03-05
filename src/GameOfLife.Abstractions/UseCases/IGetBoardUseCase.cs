using GameOfLife.Models;

namespace GameOfLife.UseCases;

public interface IGetBoardUseCase {
    Task<Board?> GetBoard(long id);
}