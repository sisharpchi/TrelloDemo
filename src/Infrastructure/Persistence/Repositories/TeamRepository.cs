using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TeamRepository(AppDbContext _context) : ITeamRepository
{
    public async Task<long> AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        await _context.Teams.AddAsync(team, cancellationToken);

        //var chat = new Chat
        //{
        //    Name = $"{team.Name} Chat",
        //    Team = team
        //};
        //await _context.Chats.AddAsync(chat, cancellationToken);

        //foreach (var member in team.UserTeams)
        //{
        //    _context.ChatUsers.Add(new ChatUser
        //    {
        //        Chat = chat,
        //        UserId = member.UserId
        //    });
        //}

        await _context.SaveChangesAsync(cancellationToken);
        return team.Id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var team = await _context.Teams.FindAsync(new object[] { id }, cancellationToken);
        if (team != null)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Teams.AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Team?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .Include(t => t.UserTeams)
            .ThenInclude(m => m.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<ICollection<Team>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserTeams
            .Where(ut => ut.UserId == userId)
            .Include(ut => ut.Team)
                .ThenInclude(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                        .ThenInclude(u => u.Role)
            .Select(ut => ut.Team)
            .ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetWithBoardsAndChatAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .Include(t => t.Boards)
                .ThenInclude(b => b.Lists)
                .ThenInclude(l => l.Tasks)
            .Include(t => t.Chat)
                .ThenInclude(c => c.Messages)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Team?> GetWithMembersAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .Include(t => t.UserTeams)
                .ThenInclude(ut => ut.User)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Team team, CancellationToken cancellationToken = default)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
