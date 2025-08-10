using Application.Dtos.Board;

namespace Application.Abstractions.Services;

public interface IBoardService
{
    Task<long> CreateAsync(BoardCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task UpdateAsync(BoardUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<BoardDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<BoardDto>> GetByTeamIdAsync(long teamId, long thisUserId, CancellationToken cancellationToken = default);
    Task<BoardDto?> GetWithListsAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
}