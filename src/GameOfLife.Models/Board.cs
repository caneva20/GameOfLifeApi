namespace GameOfLife.Models;

public class Board {
    public long Id { get; set; }
    public ICollection<BoardCell> LiveCells { get; set; } = new List<BoardCell>();
}