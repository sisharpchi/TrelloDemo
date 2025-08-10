using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IChatRepository
{
    Task<long> AddAsync(Chat chat, CancellationToken cancellationToken = default);
    Task UpdateAsync(Chat chat, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    Task<Chat?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Chat?> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}