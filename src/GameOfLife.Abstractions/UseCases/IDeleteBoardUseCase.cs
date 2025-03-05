namespace GameOfLife.UseCases;

public interface IDeleteBoardUseCase {
    Task<bool> DeleteBoard(long id);
}