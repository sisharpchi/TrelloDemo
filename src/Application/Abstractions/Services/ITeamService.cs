using Application.Dtos.Team;
using Domain.Entities;

namespace Application.Abstractions.Services;

public interface ITeamService
{
    Task<long> CreateAsync(TeamCreateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task UpdateAsync(TeamUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default);

    Task<TeamDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default);
    Task<ICollection<TeamDto>> GetByUserIdAsync(long thisUserId, CancellationToken cancellationToken = default);

    Task AddMemberAsync(long teamId, long addedUserId, long thisUserId, CancellationToken cancellationToken = default);
    Task RemoveMemberAsync(long teamId, long removedUserId, long thisUserId, CancellationToken cancellationToken = default);
}