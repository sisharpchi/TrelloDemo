using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ITaskItemRepository
{
    Task<long> AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<TaskItem>> GetByListColumnIdAsync(long listColumnId, CancellationToken cancellationToken = default);
    Task<ICollection<TaskItem>> GetByAssigneeIdAsync(long userId, CancellationToken cancellationToken = default);
}