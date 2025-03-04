using GameOfLife.Models;
using GameOfLife.Repositories;
using Microsoft.Extensions.Logging;

namespace GameOfLife.UseCases;

public sealed class CreateBoardUseCase : ICreateBoardUseCase {
    private readonly ILogger<CreateBoardUseCase> _logger;
    private readonly IGameRepository _repository;

    public CreateBoardUseCase(ILogger<CreateBoardUseCase> logger, IGameRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Board?> CreateBoard(Board board) {
        _logger.LogDebug("Creating board");

        try {
            var createdBoard = await _repository.CreateBoard(board);

            _logger.LogInformation("Created board {BoardId}", createdBoard.Id);

            return createdBoard;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating board");

            return null;
        }
    }
}