using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IAttachmentRepository
{
    Task<long> AddAsync(Attachment attachment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Attachment attachment, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<Attachment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<Attachment>> GetByTaskIdAsync(long taskId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}