using Application.Dtos.Comment;

namespace Application.Abstractions.Services;

public interface ICommentService
{
    Task<CommentResponseDto> CreateAsync(CommentCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task<CommentResponseDto> UpdateAsync(CommentUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<CommentResponseDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<CommentResponseDto>> GetByTaskIdAsync(long taskId, long thisUserId, CancellationToken cancellationToken = default);
}
