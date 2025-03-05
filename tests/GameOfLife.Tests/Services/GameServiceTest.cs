using GameOfLife.Models;
using GameOfLife.Services;

namespace GameOfLife.Tests.Services;

public class GameServiceTest {
    private readonly GameService _sut;

    private readonly Fixture _fixture = new();

    public GameServiceTest() {
        _sut = new GameService();
    }

    [Fact]
    public void BuildState_WithValidBoard_CreatesState() {
        // Arrange
        var board = _fixture.Create<Board>();

        // Act
        var state = _sut.BuildState(board, 10);

        // Assert
        state.ShouldNotBeNull();
    }

    [Fact]
    public void IsAlive_WithLiveCell_ReturnsTrue() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));

        // Act
        var isAlive = _sut.IsAlive(state, (0, 0));

        // Assert
        isAlive.ShouldBeTrue();
    }

    [Fact]
    public void IsAlive_WithDeadCell_ReturnsFalse() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));

        // Act
        var isAlive = _sut.IsAlive(state, (1, 1));

        // Assert
        isAlive.ShouldBeFalse();
    }

    [Fact]
    public void IsComplete_WithSimpleCompleteState_ReturnsTrue() {
        // Arrange
        var state = new BoardState(10);
        state.HashHistory.Enqueue(17); //Hash for 0 live cells is 17

        // Act
        var isComplete = _sut.IsComplete(state);

        // Assert
        isComplete.ShouldBeTrue();
    }

    [Fact]
    public void IsComplete_WithComplexCompleteState_ReturnsTrue() {
        // Arrange
        var state = new BoardState(10);
        state.HashHistory.Enqueue(1);
        state.HashHistory.Enqueue(2);
        state.HashHistory.Enqueue(17); //Hash for 0 live cells is 17
        state.HashHistory.Enqueue(3);
        state.HashHistory.Enqueue(4);

        // Act
        var isComplete = _sut.IsComplete(state);

        // Assert
        isComplete.ShouldBeTrue();
    }

    [Fact]
    public void ComputeHash_WithoutLiveCells_ReturnsExpectedHash() {
        // Arrange
        var state = new BoardState(10);

        // Act
        var hash = _sut.ComputeHash(state);

        // Assert
        hash.ShouldBe(17);
    }

    [Fact]
    public void ComputeHash_WithLiveCells_ReturnsExpectedHash() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));

        // Act
        var hash = _sut.ComputeHash(state);

        // Assert
        hash.ShouldBe(16337);
    }

    [Fact]
    public void UpdateLiveCells_WithValidState_EnqueuesHash() {
        // Arrange
        var state = new BoardState(10);

        // Act
        _sut.UpdateLiveCells(state, new List<(int x, int y)>());

        // Assert
        state.HashHistory.ShouldNotBeEmpty();
    }

    [Fact]
    public void UpdateLiveCells_WithValidState_UpdatesLiveCells() {
        // Arrange
        var state = new BoardState(10);

        var newLiveCells = new List<(int x, int y)>() {
            (0, 0)
        };

        // Act
        _sut.UpdateLiveCells(state, newLiveCells);

        // Assert
        state.LiveCells.ShouldHaveSingleItem();
        state.LiveCells.ShouldContain(newLiveCells[0]);
    }

    [Fact]
    public void UpdateLiveCells_WithMaxHistorySize_DequeuesOldestHash() {
        // Arrange
        var state = new BoardState(2);
        state.HashHistory.Enqueue(1);
        state.HashHistory.Enqueue(2);

        // Act
        _sut.UpdateLiveCells(state, new List<(int x, int y)>());

        // Assert
        state.HashHistory.ShouldNotContain(1);
    }

    [Fact]
    public void UpdateNeighbors_WithValidState_UpdatesNeighbors() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));

        // Act
        _sut.UpdateNeighbours(state);

        // Assert
        state.Neighbors.ShouldNotBeEmpty();
    }

    [Fact]
    public void Simulate_WithNoNeighbours_KillsAllLiveCells() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));

        // Act
        _sut.Simulate(state, 1);

        // Assert
        state.LiveCells.ShouldBeEmpty();
    }

    [Fact]
    public void Simulate_WithTwoNeighbours_KeepsAllLiveCells() {
        // Arrange
        var state = new BoardState(10);
        state.LiveCells.Add((0, 0));
        state.LiveCells.Add((1, 0));
        state.LiveCells.Add((2, 0));

        _sut.UpdateNeighbours(state);

        // Act
        _sut.Simulate(state, 1);

        // Assert
        state.LiveCells.Count.ShouldBe(3);
    }

    [Fact]
    public void Simulate_WithNoLiveCells_DoesNotThrow() {
        // Arrange
        var state = new BoardState(10);

        _sut.UpdateNeighbours(state);

        // Act + Assert
        Should.NotThrow(() => _sut.Simulate(state, 1));
    }
}