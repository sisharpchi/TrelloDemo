using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AttachmentRepository(AppDbContext _context) : IAttachmentRepository
{
    public async Task<long> AddAsync(Attachment attachment, CancellationToken cancellationToken = default)
    {
        await _context.Attachments.AddAsync(attachment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return attachment.Id;
    }

    public async Task UpdateAsync(Attachment attachment, CancellationToken cancellationToken = default)
    {
        _context.Attachments.Update(attachment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Attachments.FindAsync(new object[] { id }, cancellationToken);
        if (entity is not null)
        {
            _context.Attachments.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Attachment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ICollection<Attachment>> GetByTaskIdAsync(long taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .AsNoTracking()
            .Where(x => x.TaskItemId == taskId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}