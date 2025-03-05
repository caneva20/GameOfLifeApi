using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.UseCases;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace GameOfLife.Tests.UseCases;

[TestSubject(typeof(DeleteBoardUseCase))]
public class DeleteBoardUseCaseTest {
    private readonly DeleteBoardUseCase _sut;
    private readonly IGameRepository _gameRepositorySub;

    private readonly Fixture _fixture = new();

    public DeleteBoardUseCaseTest() {
        _gameRepositorySub = Substitute.For<IGameRepository>();

        _sut = new DeleteBoardUseCase(Substitute.For<ILogger<DeleteBoardUseCase>>(), _gameRepositorySub);
    }

    [Fact]
    public async Task DeleteBoard_WithValidId_RemovesStoredBoard() {
        //Arrange
        var board = _fixture.Create<Board>();

        _gameRepositorySub.GetBoard(Arg.Any<long>()).Returns(board);

        //Act
        var boardDeleted = await _sut.DeleteBoard(1);

        //Assert
        await _gameRepositorySub.Received(1).DeleteBoard(board);
        boardDeleted.ShouldBe(true);
    }

    [Fact]
    public async Task DeleteBoard_WithInvalidId_ReturnsFalse() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ReturnsNull();

        //Act
        var boardDeleted = await _sut.DeleteBoard(999);

        //Assert
        boardDeleted.ShouldBe(false);
    }

    [Fact]
    public async Task DeleteBoard_WithRepositoryFailure_ReturnsFalse() {
        // Arrange
        _gameRepositorySub.GetBoard(Arg.Any<long>()).ThrowsAsync<Exception>();

        // Act
        var createdBoard = await _sut.DeleteBoard(1);

        // Assert
        createdBoard.ShouldBe(false);
    }
}