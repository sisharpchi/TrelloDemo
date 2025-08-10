using Application.Dtos.ListColumn;

namespace Application.Abstractions.Services;

public interface IListColumnService
{
    Task<long> CreateAsync(ListColumnCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task UpdateAsync(ListColumnUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);

    Task<ListColumnDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<ListColumnDto>> GetByBoardIdAsync(long boardId, long thisUserId, CancellationToken cancellationToken = default);

    Task<ListColumnWithTasksDto?> GetWithTasksAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
}