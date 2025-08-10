using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IMessageRepository
{
    Task<long> AddAsync(Message message, CancellationToken cancellationToken = default);
    Task UpdateAsync(Message message, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<Message?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<Message>> GetByChatIdAsync(long chatId, CancellationToken cancellationToken = default);
}