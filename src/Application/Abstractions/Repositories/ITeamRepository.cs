using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<Team>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<long> AddAsync(Team team, CancellationToken cancellationToken = default);
    Task UpdateAsync(Team team, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<Team?> GetWithMembersAsync(long id, CancellationToken cancellationToken = default);
    Task<Team?> GetWithBoardsAndChatAsync(long id, CancellationToken cancellationToken = default);
}
