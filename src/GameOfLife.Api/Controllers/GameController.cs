using GameOfLife.Api.Dtos;
using GameOfLife.Api.Mapping;
using GameOfLife.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase {
    private readonly ILogger<GameController> _logger;

    private readonly ICreateBoardUseCase _createBoardUseCase;

    public GameController(ILogger<GameController> logger, ICreateBoardUseCase createBoardUseCase) {
        _logger = logger;
        _createBoardUseCase = createBoardUseCase;
    }

    /// <summary>
    /// Creates a board
    /// </summary>
    /// <param name="board">The board to create with initial state</param>
    /// <returns>The created board</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBoard([FromBody] BoardDto board) {
        _logger.LogInformation("Creating board");

        var createdBoard = await _createBoardUseCase.CreateBoard(board.ToEntity());

        if (createdBoard == null) {
            return BadRequest();
        }

        return Ok(); //TODO: Return 201
    }
}