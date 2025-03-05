using GameOfLife.Models;
using GameOfLife.Repositories;
using Microsoft.Extensions.Logging;

namespace GameOfLife.UseCases;

public sealed class GetBoardUseCase : IGetBoardUseCase {
    private readonly ILogger<GetBoardUseCase> _logger;
    private readonly IGameRepository _repository;

    public GetBoardUseCase(ILogger<GetBoardUseCase> logger, IGameRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Board?> GetBoard(long id) {
        try {
            return await _repository.GetBoard(id);
        }
        catch (Exception e) {
            _logger.LogError(e, "Failed to get board {Id}", id);

            return null;
        }
    }
}