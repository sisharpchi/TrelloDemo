using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ICommentRepository
{
    Task<long> AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<Comment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ICollection<Comment>> GetByTaskIdAsync(long taskId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}