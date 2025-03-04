using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.UseCases;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;

namespace GameOfLife.Tests.UseCases;

[TestSubject(typeof(CreateBoardUseCase))]
public class CreateBoardUseCaseTest {
    private readonly CreateBoardUseCase _sut;
    private readonly IGameRepository _gameRepositorySub;

    private readonly Fixture _fixture = new();

    public CreateBoardUseCaseTest() {
        _gameRepositorySub = Substitute.For<IGameRepository>();

        _sut = new CreateBoardUseCase(Substitute.For<ILogger<CreateBoardUseCase>>(), _gameRepositorySub);
    }


    [Fact]
    public async Task CreateBoard_WithValidBoard_CreatesBoard() {
        //Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.CreateBoard(board).Returns(_fixture.Create<Board>());

        //Act
        var createdBoard = await _sut.CreateBoard(board);

        //Assert
        await _gameRepositorySub.Received(1).CreateBoard(board);
        createdBoard.ShouldNotBeNull();
    }

    [Fact]
    public async Task CreateBoard_WithRepositoryFailure_ReturnsNull() {
        // Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.CreateBoard(Arg.Any<Board>()).ThrowsAsync<Exception>();

        // Act
        var createdBoard = await _sut.CreateBoard(board);

        // Assert
        createdBoard.ShouldBeNull();
    }
}