//using Application.Abstractions.Repositories;
//using Application.Abstractions.Services;
//using Application.Dtos.Board;
//using Domain.Entities;

//namespace Application.Services;

//public class BoardService(IBoardRepository _boardRepository, ITeamRepository _teamRepository) : IBoardService
//{
//    public async Task<long> CreateAsync(BoardCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var team = await _teamRepository.GetByIdAsync(dto.TeamId, cancellationToken);

//        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
//            throw new UnauthorizedAccessException("Siz ushbu teamga board yarata olmaysiz");

//        var board = new Board
//        {
//            Name = dto.Name,
//            Description = dto.Description,
//            TeamId = dto.TeamId
//        };

//        return await _boardRepository.AddAsync(board, cancellationToken);
//    }

//    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var board = await _boardRepository.GetByIdAsync(id, cancellationToken);
//        if (board is null)
//            throw new KeyNotFoundException("Board topilmadi");

//        if (!board.Team.UserTeams.Any(u => u.UserId == thisUserId))
//            throw new UnauthorizedAccessException("Siz bu boardni o‘chira olmaysiz");

//        await _boardRepository.DeleteAsync(id, cancellationToken);
//    }

//    public async Task<BoardDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var board = await _boardRepository.GetByIdAsync(id, cancellationToken);

//        if (board is null || !board.Team.UserTeams.Any(u => u.UserId == thisUserId))
//            return null;

//        return ConvertEntityToDto(board);
//    }

//    public async Task<ICollection<BoardDto>> GetByTeamIdAsync(long teamId, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var team = await _teamRepository.GetByIdAsync(teamId, cancellationToken);
//        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
//            throw new UnauthorizedAccessException("Siz ushbu teamdagi boardlarni ko‘ra olmaysiz");

//        var boards = await _boardRepository.GetByTeamIdAsync(teamId, cancellationToken);
//        return boards.Select(ConvertEntityToDto).ToList();
//    }

//    public async Task<BoardDto?> GetWithListsAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var board = await _boardRepository.GetWithListsAsync(id, cancellationToken);
//        if (board is null || !board.Team.UserTeams.Any(u => u.UserId == thisUserId))
//            return null;

//        return ConvertEntityToDto(board, includeLists: true);
//    }

//    public async Task UpdateAsync(BoardUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
//    {
//        var board = await _boardRepository.GetByIdAsync(dto.Id, cancellationToken);
//        if (board is null)
//            throw new KeyNotFoundException("Board topilmadi");

//        if (!board.Team.UserTeams.Any(u => u.UserId == thisUserId))
//            throw new UnauthorizedAccessException("Siz bu boardni o‘zgartira olmaysiz");

//        board.Name = dto.Name;
//        board.Description = dto.Description;

//        await _boardRepository.UpdateAsync(board, cancellationToken);
//    }

//    private static BoardDto ConvertEntityToDto(Board board, bool includeLists = false)
//    {
//        return new BoardDto
//        {
//            Id = board.Id,
//            Name = board.Name,
//            Description = board.Description,
//            TeamId = board.TeamId,
//            Lists = includeLists
//                ? board.ListColumns.Select(l => new BoardListDto
//                {
//                    Id = l.Id,
//                    Name = l.Name,
//                    Tasks = l.Tasks.Select(t => new BoardTaskDto
//                    {
//                        Id = t.Id,
//                        Title = t.Title,
//                        Description = t.Description
//                    }).ToList()
//                }).ToList()
//                : null
//        };
//    }
//}