using GameOfLife.Models;

namespace GameOfLife.UseCases;

public interface ICreateBoardUseCase {
    Task<Board?> CreateBoard(Board board);
}