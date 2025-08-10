using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.ListColumn;
using Application.Dtos.TaskItem;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ListColumnService(IListColumnRepository _listColumnRepository, IBoardRepository _boardRepository, ITeamRepository _teamRepository) : IListColumnService
{
    public async Task<long> CreateAsync(ListColumnCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(dto.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu boardga list-column yarata olmaysiz.");
        }

        var listColumn = ConvertDtoToListColumnEntity(dto);
        listColumn.Order = board.Lists.Count + 1;

        return await _listColumnRepository.AddAsync(listColumn, cancellationToken);
    }

    public async Task UpdateAsync(ListColumnUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetByIdAsync(dto.Id, cancellationToken)
            ?? throw new KeyNotFoundException("ListColumn topilmadi.");

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu list-columnni o‘zgartira olmaysiz.");
        }

        listColumn.Name = dto.Name;
        listColumn.Order = listColumn.Order;

        await _listColumnRepository.UpdateAsync(listColumn, cancellationToken);
    }

    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("ListColumn topilmadi.");

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u =>
            u.UserId == thisUserId &&
            (u.Role == TeamRole.Owner || u.Role == TeamRole.Admin)))
        {
            throw new UnauthorizedAccessException("Siz ushbu list-columnni o‘chira olmaysiz.");
        }

        await _listColumnRepository.DeleteAsync(listColumn.Id, cancellationToken);
    }

    public async Task<ListColumnDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetByIdAsync(id, cancellationToken);
        if (listColumn == null) return null;

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz bu list-columnni ko‘ra olmaysiz.");

        return ConvertEntityToListColumnDto(listColumn);
    }

    public async Task<ICollection<ListColumnDto>> GetByBoardIdAsync(long boardId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz bu boarddagi list-columnlarni ko‘ra olmaysiz.");

        var listColumns = await _listColumnRepository.GetByBoardIdAsync(boardId, cancellationToken);

        return listColumns.Select(lc => ConvertEntityToListColumnDto(lc)).ToList();
    }

    public async Task<ListColumnWithTasksDto?> GetWithTasksAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var listColumn = await _listColumnRepository.GetWithTasksAsync(id, cancellationToken);
        if (listColumn == null) return null;

        var board = await _boardRepository.GetByIdAsync(listColumn.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException("Board topilmadi.");

        var team = await _teamRepository.GetByIdAsync(board.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException("Team topilmadi.");

        if (!team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz bu list-columnni ko‘ra olmaysiz.");

        return ConvertEntityToListColumnWithTasksDto(listColumn);
    }

    private ListColumn ConvertDtoToListColumnEntity(ListColumnCreateDto dto)
    {
        return new ListColumn()
        {
            Name = dto.Name,
            BoardId = dto.BoardId,
        };
    }

    private ListColumnDto ConvertEntityToListColumnDto(ListColumn listColumn)
    {
        return new ListColumnDto()
        {
            Id = listColumn.Id,
            Name = listColumn.Name,
            BoardId = listColumn.BoardId,
            Order = listColumn.Order
        };
    }

    private ListColumnWithTasksDto ConvertEntityToListColumnWithTasksDto(ListColumn listColumn)
    {
        return new ListColumnWithTasksDto
        {
            Id = listColumn.Id,
            Name = listColumn.Name,
            BoardId = listColumn.BoardId,
            Order = listColumn.Order,
            Tasks = listColumn.Tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ListColumnId = t.ListColumnId,
                CreatedAt = t.CreatedAt,
                AssignedUserId = t.AssignedToId
            }).ToList()
        };
    }
}