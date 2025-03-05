using GameOfLife.Api.Dtos;
using GameOfLife.Api.Mapping;
using GameOfLife.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase {
    private readonly ILogger<GameController> _logger;

    private readonly IGetBoardUseCase _getBoardUseCase;
    private readonly ICreateBoardUseCase _createBoardUseCase;

    public GameController(ILogger<GameController> logger, ICreateBoardUseCase createBoardUseCase, IGetBoardUseCase getBoardUseCase) {
        _logger = logger;
        _createBoardUseCase = createBoardUseCase;
        _getBoardUseCase = getBoardUseCase;
    }

    /// <summary>
    /// Fetches a game board
    /// </summary>
    /// <param name="id">The id of the board</param>
    /// <returns>The game board</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BoardDto?>> GetBoard(long id) {
        var board = await _getBoardUseCase.GetBoard(id);

        if (board == null) {
            return NotFound();
        }

        return Ok(board.ToDto());
    }

    /// <summary>
    /// Creates a board
    /// </summary>
    /// <param name="board">The board to create with initial state</param>
    /// <returns>The created board</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBoard([FromBody] BoardDto board) {
        _logger.LogInformation("Creating board");

        var createdBoard = await _createBoardUseCase.CreateBoard(board.ToEntity());

        if (createdBoard == null) {
            return BadRequest();
        }

        return CreatedAtAction("GetBoard", new { id = createdBoard.Id }, createdBoard.ToDto());
    }
}