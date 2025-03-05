namespace GameOfLife.Models;

public class BoardState {
    public HashSet<(int x, int y)> LiveCells { get; } = new();
    public Dictionary<(int x, int y), int> Neighbors { get; } = new();
    public Queue<int> HashHistory { get; } = new();

    public int MaxStateHistory { get; }

    public BoardState(int maxStateHistory) {
        MaxStateHistory = maxStateHistory;
    }
}