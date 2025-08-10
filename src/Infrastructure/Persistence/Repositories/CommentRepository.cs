using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CommentRepository(AppDbContext _context) : ICommentRepository
{
    public async Task<long> AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Comments.FindAsync(new object[] { id }, cancellationToken);
        if (entity is not null)
        {
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<ICollection<Comment>> GetByTaskIdAsync(long taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.TaskItemId == taskId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}