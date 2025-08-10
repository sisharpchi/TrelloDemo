using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ListColumnRepository(AppDbContext _context) : IListColumnRepository
{
    public async Task<long> AddAsync(ListColumn listColumn, CancellationToken cancellationToken = default)
    {
        await _context.ListColumns.AddAsync(listColumn, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return listColumn.Id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ListColumns.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _context.ListColumns.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ListColumns.AnyAsync(lc => lc.Id == id, cancellationToken);
    }

    public async Task<ICollection<ListColumn>> GetByBoardIdAsync(long boardId, CancellationToken cancellationToken = default)
    {
        return await _context.ListColumns
            .Where(lc => lc.BoardId == boardId)
            .OrderBy(lc => lc.Order) // columnlar tartibi bo‘lishi uchun
            .ToListAsync(cancellationToken);
    }

    public async Task<ListColumn?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ListColumns
            .FirstOrDefaultAsync(lc => lc.Id == id, cancellationToken);
    }

    public async Task<ListColumn?> GetWithTasksAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ListColumns
            .Include(lc => lc.Tasks)
            .FirstOrDefaultAsync(lc => lc.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(ListColumn listColumn, CancellationToken cancellationToken = default)
    {
        _context.ListColumns.Update(listColumn);
        await _context.SaveChangesAsync(cancellationToken);
    }
}