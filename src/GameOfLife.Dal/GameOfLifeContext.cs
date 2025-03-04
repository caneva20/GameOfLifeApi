using GameOfLife.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife;

public class GameOfLifeContext : DbContext {
    public DbSet<Board> Boards { get; set; }

    public GameOfLifeContext(DbContextOptions<GameOfLifeContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}