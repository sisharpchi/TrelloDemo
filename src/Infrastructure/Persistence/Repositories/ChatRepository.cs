using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChatRepository(AppDbContext _context) : IChatRepository
{
    public async Task<long> AddAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return chat.Id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var chat = await _context.Chats.FindAsync(new object?[] { id }, cancellationToken);
        if (chat != null)
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Chats.AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Chat?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Chats
            .Include(c => c.Messages) 
            .Include(c => c.ChatUsers)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Chat?> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Chats
            .Include(c => c.Messages)
            .Include(c => c.ChatUsers)
            .FirstOrDefaultAsync(c => c.TeamId == teamId, cancellationToken);
    }

    public async Task UpdateAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        _context.Chats.Update(chat);
        await _context.SaveChangesAsync(cancellationToken);
    }
}