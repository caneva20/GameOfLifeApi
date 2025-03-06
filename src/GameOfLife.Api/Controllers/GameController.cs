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
    private readonly IGetBoardStateUseCase _getBoardStateUseCase;
    private readonly ICreateBoardUseCase _createBoardUseCase;
    private readonly IDeleteBoardUseCase _deleteBoardUseCase;

    public GameController(
        ILogger<GameController> logger,
        ICreateBoardUseCase createBoardUseCase,
        IGetBoardUseCase getBoardUseCase,
        IDeleteBoardUseCase deleteBoardUseCase,
        IGetBoardStateUseCase getBoardStateUseCase) {
        _logger = logger;
        _createBoardUseCase = createBoardUseCase;
        _getBoardUseCase = getBoardUseCase;
        _deleteBoardUseCase = deleteBoardUseCase;
        _getBoardStateUseCase = getBoardStateUseCase;
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
    /// Fetches a game board with its current state
    /// </summary>
    /// <param name="id">The id of the board</param>
    /// <param name="iterations">The number of iterations to simulate the board before returning the state. Defaults to 1.</param>
    /// <param name="requireCompletion">If set to true, the board must have completed a simulation iteration to return a valid state. Defaults to false.</param>
    /// <returns>The game board with its current state</returns>
    [HttpGet("{id:long}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<BoardDto?>> GetBoardState(
        long id,
        [FromQuery] int iterations = 1,
        [FromQuery] bool requireCompletion = false) {
        _logger.LogInformation("Getting board state for board {Id}", id);

        var stateResult = await _getBoardStateUseCase.GetBoardState(id, iterations);

        if (stateResult == null) {
            return NotFound();
        }

        if (requireCompletion && !stateResult.Value.complete) {
            return UnprocessableEntity("Board does not finish with the given number of iterations.");
        }

        return stateResult.Value.board.ToDto();
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

    /// <summary>
    /// Deletes a board by its id
    /// </summary>
    /// <param name="id">The id of the board to be deleted</param>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteBoard(long id) {
        _logger.LogInformation("Deleting board {Id}", id);

        var boardDeleted = await _deleteBoardUseCase.DeleteBoard(id);

        if (!boardDeleted) {
            return NotFound();
        }

        return Ok();
    }
}