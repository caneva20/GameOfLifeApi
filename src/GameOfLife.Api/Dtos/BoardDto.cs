namespace GameOfLife.Api.Dtos;

public class BoardDto {
    public long Id { get; set; }
    public ICollection<BoardCellDto> LiveCells { get; set; } = new List<BoardCellDto>();
}