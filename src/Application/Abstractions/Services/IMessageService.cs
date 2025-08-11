using Application.Dtos.Message;

namespace Application.Abstractions.Services;

public interface IMessageService
{
    Task<long> CreateAsync(MessageCreateDto dto, long senderId, CancellationToken cancellationToken = default);
    Task<MessageDto> UpdateAsync(MessageUpdateDto dto, long senderId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long senderId, CancellationToken cancellationToken = default);

    Task<MessageDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<MessageDto>> GetByChatIdAsync(long chatId, long thisUserId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
