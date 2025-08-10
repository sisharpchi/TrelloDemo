using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IListColumnRepository
{
    Task<long> AddAsync(ListColumn listColumn, CancellationToken cancellationToken = default);
    Task UpdateAsync(ListColumn listColumn, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<ListColumn?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<ListColumn>> GetByBoardIdAsync(long boardId, CancellationToken cancellationToken = default);
    Task<ListColumn?> GetWithTasksAsync(long id, CancellationToken cancellationToken = default);
}