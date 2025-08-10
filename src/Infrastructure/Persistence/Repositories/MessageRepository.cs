using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MessageRepository(AppDbContext _context) : IMessageRepository
{
    public async Task<long> AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return message.Id;
    }

    public async Task UpdateAsync(Message message, CancellationToken cancellationToken = default)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Messages.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.AnyAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<ICollection<Message>> GetByChatIdAsync(long chatId, CancellationToken cancellationToken = default)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .ToListAsync(cancellationToken);
    }
}