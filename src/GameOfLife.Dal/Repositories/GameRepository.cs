using GameOfLife.Models;

namespace GameOfLife.Repositories;

public sealed class GameRepository : IGameRepository {
    private readonly GameOfLifeContext _context;

    public GameRepository(GameOfLifeContext context) {
        _context = context;
    }

    public async Task<Board?> GetBoard(long boardId) {
        return await _context.Boards.FindAsync(boardId);
    }

    public async Task<Board> CreateBoard(Board board) {
        _context.Add(board);

        await _context.SaveChangesAsync();

        return board;
    }
}