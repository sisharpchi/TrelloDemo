using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.Comment;
using Application.Helpers.SignalR;
using Core.Errors;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class CommentService(ICommentRepository _commentRepository, ITaskItemRepository _taskItemRepository, ITeamRepository _teamRepository, IHubContext<NotificationHub> _hub) : ICommentService
{
    public async Task<CommentResponseDto> CreateAsync(CommentCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var task = await _taskItemRepository.GetByIdAsync(dto.TaskItemId, cancellationToken);
        if (task == null || !task.ListColumn.Board.Team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new EntityNotFoundException("Task not found or you can't write comment this task");
        
        var comment = new Comment
        {
            TaskItemId = dto.TaskItemId,
            AuthorId = thisUserId,
            Content = dto.Content,
        };

        await _commentRepository.AddAsync(comment, cancellationToken);

        var response = ConvertToDto(comment);

        await _hub.Clients.Group($"task-{dto.TaskItemId}")
            .SendAsync("CommentCreated", response, cancellationToken);

        return response;
    }

    public async Task<CommentResponseDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            throw new EntityNotFoundException("Comment not found");

        var task = await _taskItemRepository.GetByIdAsync(comment.TaskItemId, cancellationToken);
        if (task == null || !task.ListColumn.Board.Team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new EntityNotFoundException("Task not found or you can't get");

        return ConvertToDto(comment);
    }

    private CommentResponseDto ConvertToDto(Comment comment)
    {
        return new CommentResponseDto()
        {
            Id = comment.Id,
            TaskItemId = comment.TaskItemId,
            AuthorId = comment.AuthorId,
            Content = comment.Content,
            AuthorName = comment.Author.UserName,
            CreatedAt = comment.CreatedAt,
            ParentCommentId = comment.ParentCommentId,
            Replies = comment.ReplyComments.Select(r => ConvertToDto(r)).ToList()
        };
    }

    public async Task<ICollection<CommentResponseDto>> GetByTaskIdAsync(long taskId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByTaskIdAsync(taskId, cancellationToken);
        var task = await _taskItemRepository.GetByIdAsync(taskId, cancellationToken);
        if (task == null || !task.ListColumn.Board.Team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new EntityNotFoundException("Task not found or you can't write comment this task");

        return comments.Select(c => ConvertToDto(c)).ToList();
    }

    public async Task<CommentResponseDto> UpdateAsync(CommentUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (comment == null)
            throw new EntityNotFoundException("Comment not found");

        var task = await _taskItemRepository.GetByIdAsync(comment.TaskItemId, cancellationToken);
        var userTeam = task.ListColumn.Board.Team.UserTeams.FirstOrDefault(u => u.UserId == thisUserId);

        if (comment.AuthorId != thisUserId || (userTeam.Role != TeamRole.Owner || userTeam.Role != TeamRole.Admin));
            throw new ForbiddenException("You cannot edit this comment");

        comment.Content = dto.Content;
        comment.CreatedAt = DateTime.UtcNow;

        await _commentRepository.UpdateAsync(comment, cancellationToken);

        var response = ConvertToDto(comment);

        await _hub.Clients.Group($"task-{comment.TaskItemId}")
            .SendAsync("CommentUpdated", response, cancellationToken);

        return response;
    }

    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            throw new EntityNotFoundException("Comment not found");
                
        var task = await _taskItemRepository.GetByIdAsync(comment.TaskItemId, cancellationToken);
        var userTeam = task.ListColumn.Board.Team.UserTeams.FirstOrDefault(u => u.UserId == thisUserId);

        if (comment.AuthorId != thisUserId || (userTeam.Role != TeamRole.Owner || userTeam.Role != TeamRole.Admin)) ;
            throw new ForbiddenException("You cannot edit this comment");

        await _commentRepository.DeleteAsync(comment.Id, cancellationToken);

        await _hub.Clients.Group($"task-{comment.TaskItemId}")
            .SendAsync("CommentDeleted", id, cancellationToken);
    }
}