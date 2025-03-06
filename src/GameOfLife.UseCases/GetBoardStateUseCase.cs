using GameOfLife.Models;
using GameOfLife.Models.Options;
using GameOfLife.Repositories;
using GameOfLife.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameOfLife.UseCases;

public sealed class GetBoardStateUseCase : IGetBoardStateUseCase {
    private readonly ILogger<GetBoardStateUseCase> _logger;
    private readonly IGameRepository _repository;
    private readonly IGameService _service;

    private readonly SimulationOptions _options;

    public GetBoardStateUseCase(
        ILogger<GetBoardStateUseCase> logger,
        IGameRepository repository,
        IGameService service,
        IOptions<SimulationOptions> simulationOptions) {
        _logger = logger;
        _repository = repository;
        _service = service;

        _options = simulationOptions.Value;
    }

    public async Task<(Board board, bool complete)?> GetBoardState(long id, int iterations) {
        try {
            var board = await _repository.GetBoard(id);

            if (board == null) {
                return null;
            }

            var boardState = _service.BuildState(board, _options.HistorySize);

            _service.Simulate(boardState, iterations);

            board.LiveCells = boardState.LiveCells.Select(x => new BoardCell { X = x.x, Y = x.y }).ToList();

            return (board, _service.IsComplete(boardState));
        }
        catch (Exception e) {
            _logger.LogError(e, "Failed to get board state for board {Id}", id);

            return null;
        }
    }
}