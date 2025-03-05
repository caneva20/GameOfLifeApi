using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.UseCases;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace GameOfLife.Tests.UseCases;

[TestSubject(typeof(GetBoardUseCase))]
public class GetBoardUseCaseTest {
    private readonly GetBoardUseCase _sut;
    private readonly IGameRepository _gameRepositorySub;

    private readonly Fixture _fixture = new();

    public GetBoardUseCaseTest() {
        _gameRepositorySub = Substitute.For<IGameRepository>();

        _sut = new GetBoardUseCase(Substitute.For<ILogger<GetBoardUseCase>>(), _gameRepositorySub);
    }


    [Fact]
    public async Task GetBoard_WithValidId_ReturnStoredBoard() {
        //Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.GetBoard(Arg.Any<long>()).Returns(board);

        //Act
        var foundBoard = await _sut.GetBoard(1);

        //Assert
        foundBoard.ShouldBe(board);
    }

    [Fact]
    public async Task GetBoard_WithInvalidId_ReturnsNull() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ReturnsNull();

        // Act
        var createdBoard = await _sut.GetBoard(999);

        // Assert
        createdBoard.ShouldBeNull();
    }

    [Fact]
    public async Task GetBoard_WithRepositoryFailure_ReturnsNull() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ThrowsAsync<Exception>();

        // Act
        var createdBoard = await _sut.GetBoard(1);

        // Assert
        createdBoard.ShouldBeNull();
    }
}