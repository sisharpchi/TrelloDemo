using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BoardRepository(AppDbContext _context) : IBoardRepository
{
    public async Task<long> AddAsync(Board board, CancellationToken cancellationToken = default)
    {
        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return board.Id;
    }

    public async Task UpdateAsync(Board board, CancellationToken cancellationToken = default)
    {
        _context.Boards.Update(board);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var board = await _context.Boards.FindAsync(new object[] { id }, cancellationToken);
        if (board != null)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Boards.AnyAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<Board?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .Include(b => b.Team)
                .ThenInclude(b => b.UserTeams)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<ICollection<Board>?> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .Where(b => b.TeamId == teamId)
            .Include(b => b.Team)
                .ThenInclude(t => t.UserTeams)
            .ToListAsync(cancellationToken);
    }

    public async Task<Board?> GetWithListsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .Include(b => b.Lists)
                .ThenInclude(l => l.Tasks)
            .Include(b => b.Team)
                .ThenInclude(t => t.UserTeams)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }
}