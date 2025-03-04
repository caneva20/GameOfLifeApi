namespace GameOfLife.Models;

public class Board {
    public long Id { get; set; }
    public BoardCell[] LiveCells { get; set; } = Array.Empty<BoardCell>();
}