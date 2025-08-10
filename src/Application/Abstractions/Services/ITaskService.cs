using Application.Dtos.TaskItem;

namespace Application.Abstractions.Services;

public interface ITaskService
{
    Task<long> CreateAsync(TaskItemCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItemUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);

    Task<TaskItemDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<TaskItemDto>> GetByListColumnIdAsync(long listColumnId, long thisUserId, CancellationToken cancellationToken = default);

    Task MoveToListAsync(long taskId, long targetListId, long thisUserId, CancellationToken cancellationToken = default);
    Task AssignUserAsync(long taskId, long assignedUserId, long thisUserId, CancellationToken cancellationToken = default);
    Task UnassignUserAsync(long taskId, long removedUserId, long thisUserId, CancellationToken cancellationToken = default);
}