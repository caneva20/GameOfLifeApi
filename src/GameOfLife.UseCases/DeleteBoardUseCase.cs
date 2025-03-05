using GameOfLife.Repositories;
using Microsoft.Extensions.Logging;

namespace GameOfLife.UseCases;

public sealed class DeleteBoardUseCase : IDeleteBoardUseCase {
    private readonly ILogger<DeleteBoardUseCase> _logger;
    private readonly IGameRepository _repository;

    public DeleteBoardUseCase(ILogger<DeleteBoardUseCase> logger, IGameRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    public async Task<bool> DeleteBoard(long id) {
        try {
            var board = await _repository.GetBoard(id);

            if (board == null) {
                return false;
            }

            await _repository.DeleteBoard(board);

            return true;
        }
        catch (Exception e) {
            _logger.LogError(e, "Failed to delete board {Id}", id);

            return false;
        }
    }
}