using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TaskItemRepository(AppDbContext _context) : ITaskItemRepository
{
    public async Task<long> AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        await _context.TaskItems.AddAsync(taskItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return taskItem.Id;
    }

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TaskItems.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _context.TaskItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems.AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .Include(t => t.AssignedTo)
            .Include(t => t.ListColumn)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<ICollection<TaskItem>> GetByListColumnIdAsync(long listColumnId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .Where(t => t.ListColumnId == listColumnId)
            .Include(t => t.AssignedTo)
            .OrderBy(t => t.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<TaskItem>> GetByAssigneeIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .Where(t => t.AssignedToId == userId)
            .Include(t => t.ListColumn)
            .OrderBy(t => t.DueDate)
            .ToListAsync(cancellationToken);
    }
}