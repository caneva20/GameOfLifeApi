using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.Services;
using GameOfLife.UseCases;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace GameOfLife.Tests.UseCases;

[TestSubject(typeof(GetBoardStateUseCase))]
public class GetBoardStateUseCaseTest {
    private readonly GetBoardStateUseCase _sut;
    private readonly IGameRepository _gameRepositorySub;
    private readonly IGameService _gameServiceSub;

    private readonly Fixture _fixture = new();

    public GetBoardStateUseCaseTest() {
        _gameRepositorySub = Substitute.For<IGameRepository>();
        _gameServiceSub = Substitute.For<IGameService>();

        _sut = new GetBoardStateUseCase(Substitute.For<ILogger<GetBoardStateUseCase>>(), _gameRepositorySub, _gameServiceSub);
    }

    [Fact]
    public async Task GetBoardState_WithValidId_ReturnStoredBoard() {
        //Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.GetBoard(Arg.Any<long>()).Returns(board);
        _gameServiceSub.BuildState(Arg.Any<Board>(), Arg.Any<int>()).Returns(new BoardState(1));

        //Act
        var foundBoard = await _sut.GetBoardState(1, 1);

        //Assert
        foundBoard.ShouldBe(board);
    }

    [Fact]
    public async Task GetBoardState_WithValidId_CallsSimulationService() {
        //Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.GetBoard(Arg.Any<long>()).Returns(board);
        _gameServiceSub.BuildState(Arg.Any<Board>(), Arg.Any<int>()).Returns(new BoardState(1));

        //Act
        _ = await _sut.GetBoardState(1, 1);

        //Assert
        _gameServiceSub.Received(1).Simulate(Arg.Any<BoardState>(), Arg.Any<int>());
    }


    [Fact]
    public async Task GetBoardState_WithInvalidId_ReturnsNull() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ReturnsNull();
        _gameServiceSub.BuildState(Arg.Any<Board>(), Arg.Any<int>()).Returns(new BoardState(1));

        // Act
        var createdBoard = await _sut.GetBoardState(999, 1);

        // Assert
        createdBoard.ShouldBeNull();
    }

    [Fact]
    public async Task GetBoardState_WithRepositoryFailure_ReturnsNull() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ThrowsAsync<Exception>();

        // Act
        var createdBoard = await _sut.GetBoardState(1, 1);

        // Assert
        createdBoard.ShouldBeNull();
    }
}