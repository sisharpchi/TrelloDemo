using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.TaskItem;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class TaskService(ITaskItemRepository _taskRepository, IListColumnRepository _listColumnRepository, IBoardRepository _boardRepository, ITeamRepository _teamRepository) : ITaskService
{
    private async Task<Team> GetTeamByTaskIdAsync(long taskId, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        var listColumn = await _listColumnRepository.GetByIdAsync(task.ListColumnId, cancellationToken)
            ?? throw new KeyNotFoundException("ListColumn topilmadi.");

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        return team;
    }

    public async Task<long> CreateAsync(TaskItemCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetByIdAsync(dto.ListColumnId, cancellationToken)
            ?? throw new KeyNotFoundException("ListColumn topilmadi.");

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu list-columnga task yarata olmaysiz.");
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ListColumnId = dto.ListColumnId
        };
         
        return await _taskRepository.AddAsync(task, cancellationToken);
    }

    public async Task UpdateAsync(TaskItemUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await GetTeamByTaskIdAsync(dto.Id, cancellationToken);

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu taskni o‘zgartira olmaysiz.");
        }

        var task = await _taskRepository.GetByIdAsync(dto.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;

        await _taskRepository.UpdateAsync(task, cancellationToken);
    }

    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await GetTeamByTaskIdAsync(id, cancellationToken);

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu taskni o‘chira olmaysiz.");
        }

        var task = await _taskRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        await _taskRepository.DeleteAsync(task, cancellationToken);
    }

    public async Task<TaskItemDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null) return null;

        var team = await GetTeamByTaskIdAsync(id, cancellationToken);

        if (!team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz bu taskni ko‘ra olmaysiz.");

        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            ListColumnId = task.ListColumnId,
            AssignedUserId = task.AssignedUserId
        };
    }

    public async Task<ICollection<TaskItemDto>> GetByListColumnIdAsync(long listColumnId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetByIdAsync(listColumnId, cancellationToken)
            ?? throw new KeyNotFoundException("ListColumn topilmadi.");

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz bu list-columndagi tasklarni ko‘ra olmaysiz.");

        var tasks = await _taskRepository.GetByListColumnIdAsync(listColumnId, cancellationToken);

        return tasks.Select(t => new TaskItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            ListColumnId = t.ListColumnId,
            AssignedUserId = t.AssignedUserId
        }).ToList();
    }

    public async Task MoveToListAsync(long taskId, long targetListId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await GetTeamByTaskIdAsync(taskId, cancellationToken);

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz taskni boshqa list-columnga o‘tkaza olmaysiz.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        var targetList = await _listColumnRepository.GetByIdAsync(targetListId, cancellationToken)
            ?? throw new KeyNotFoundException("Target ListColumn topilmadi.");

        task.ListColumnId = targetListId;

        await _taskRepository.UpdateAsync(task, cancellationToken);
    }

    public async Task AssignUserAsync(long taskId, long assignedUserId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await GetTeamByTaskIdAsync(taskId, cancellationToken);

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu taskga foydalanuvchi biriktira olmaysiz.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        if (!team.UserTeams.Any(u => u.UserId == assignedUserId))
            throw new InvalidOperationException("Berilayotgan foydalanuvchi team a’zosi emas.");

        task.AssignedUserId = assignedUserId;

        await _taskRepository.UpdateAsync(task, cancellationToken);
    }

    public async Task UnassignUserAsync(long taskId, long removedUserId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await GetTeamByTaskIdAsync(taskId, cancellationToken);

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu taskdan foydalanuvchi olib tashlay olmaysiz.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new KeyNotFoundException("Task topilmadi.");

        if (task.AssignedUserId != removedUserId)
            throw new InvalidOperationException("Berilgan foydalanuvchi ushbu taskga biriktirilmagan.");

        task.AssignedUserId = null;

        await _taskRepository.UpdateAsync(task, cancellationToken);
    }
}