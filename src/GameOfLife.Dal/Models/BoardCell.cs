using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Models;

[Owned]
public class BoardCell {
    public int X { get; set; }
    public int Y { get; set; }
}