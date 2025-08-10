using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<Board>?> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default);
    Task<long> AddAsync(Board board, CancellationToken cancellationToken = default);
    Task UpdateAsync(Board board, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<Board?> GetWithListsAsync(long id, CancellationToken cancellationToken = default); 
}
