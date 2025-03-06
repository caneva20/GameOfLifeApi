using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.Services;
using Microsoft.Extensions.Logging;

namespace GameOfLife.UseCases;

public sealed class GetBoardStateUseCase : IGetBoardStateUseCase {
    private readonly ILogger<GetBoardStateUseCase> _logger;
    private readonly IGameRepository _repository;
    private readonly IGameService _service;

    public GetBoardStateUseCase(ILogger<GetBoardStateUseCase> logger, IGameRepository repository, IGameService service) {
        _logger = logger;
        _repository = repository;
        _service = service;
    }

    public async Task<Board?> GetBoardState(long id, int iterations) {
        try {
            var board = await _repository.GetBoard(id);

            if (board == null) {
                return null;
            }

            var boardState = _service.BuildState(board, 10); //TODO: Read max history from config

            _service.Simulate(boardState, iterations);

            board.LiveCells = boardState.LiveCells.Select(x => new BoardCell { X = x.x, Y = x.y }).ToList();

            return board;
        }
        catch (Exception e) {
            _logger.LogError(e, "Failed to get board state for board {Id}", id);

            return null;
        }
    }
}