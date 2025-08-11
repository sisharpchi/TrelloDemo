using Application.Dtos.Chat;

namespace Application.Abstractions.Services;

public interface IChatService
{
    Task<long> CreateAsync(ChatCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task UpdateAsync(ChatUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ChatDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ChatDto?> GetByTeamIdAsync(long teamId, long thisUserId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}