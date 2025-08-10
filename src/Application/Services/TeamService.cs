using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.Enums;
using Application.Dtos.Team;
using Core.Errors;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class TeamService(ITeamRepository _teamRepository, IUserRepository _userRepository) : ITeamService
{
    public async Task<long> CreateAsync(TeamCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = ConvertCreateDtoToTeamEntity(dto);

        var teamId = await _teamRepository.AddAsync(team, cancellationToken);

        await AddMemberAsync(teamId, thisUserId, thisUserId, cancellationToken);

        return teamId;
    }

    public async Task UpdateAsync(TeamUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(dto.Id, cancellationToken)
                   ?? throw new KeyNotFoundException("Team not found");

        var role = team.UserTeams.FirstOrDefault(u => u.UserId == thisUserId);
        if (role.Role != (TeamRole)TeamRoleDto.Owner || role.Role != (TeamRole)TeamRoleDto.Admin)
        {
            throw new ForbiddenException("Sizga Teamni malumotlari o'zgartirishga ruxsat yo'q");
        }
        
        team.Name = dto.Name;
        team.Description = dto.Description;

        await _teamRepository.UpdateAsync(team, cancellationToken);
    }

    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new KeyNotFoundException("Team not found");

        var role = team.UserTeams.FirstOrDefault(u => u.UserId == thisUserId);
        if (role.Role != (TeamRole)TeamRoleDto.Owner || role.Role != (TeamRole)TeamRoleDto.Admin)
        {
            throw new ForbiddenException("Sizga Teamni malumotlari o'zgartirishga ruxsat yo'q");
        }

        await _teamRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<TeamDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(id, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId)) return null;

        return ConvertEntityToTeamDto(team);
    }

    public async Task<ICollection<TeamDto>> GetByUserIdAsync(long thisUserId, CancellationToken cancellationToken = default)
    {
        var teams = await _teamRepository.GetByUserIdAsync(thisUserId, cancellationToken);

        return teams
            .Select(t => ConvertEntityToTeamDto(t))
            .ToList();
    }

    public async Task AddMemberAsync(long teamId, long userId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetWithMembersAsync(teamId, cancellationToken)
                   ?? throw new KeyNotFoundException("Team not found");

        var isOwner = team.UserTeams.FirstOrDefault(m => m.UserId == thisUserId);
        if (isOwner.Role != (TeamRole)TeamRoleDto.Owner || isOwner.Role != (TeamRole)TeamRoleDto.Admin)
        {
            throw new ForbiddenException("San bu teamni owneri yoki admini emassan");
        }

        if (team.UserTeams.Any(m => m.UserId == userId))
            throw new InvalidOperationException("User already in team");

        var role = team.UserTeams.Count == 0 ? TeamRoleDto.Owner : TeamRoleDto.Member;

        team.UserTeams.Add(new UserTeam
        {
            TeamId = teamId,
            UserId = userId,
            Role = (TeamRole)role
        });

        await _teamRepository.UpdateAsync(team, cancellationToken);
    }

    public async Task RemoveMemberAsync(long teamId, long removedUserId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetWithMembersAsync(teamId, cancellationToken)
                   ?? throw new KeyNotFoundException("Team not found");

        var isOwner = team.UserTeams.FirstOrDefault(m => m.UserId == thisUserId);
        if (isOwner.Role != (TeamRole)TeamRoleDto.Owner || isOwner.Role != (TeamRole)TeamRoleDto.Admin)
        {
            throw new ForbiddenException("San bu teamni owneri yoki admini emassan");
        }

        var member = team.UserTeams.FirstOrDefault(m => m.UserId == removedUserId);
        if (member is null || member.Role == (TeamRole)TeamRoleDto.Owner)
            throw new KeyNotFoundException("User not found in team || This user Owner");

        team.UserTeams.Remove(member);

        await _teamRepository.UpdateAsync(team, cancellationToken);
    }

    private Team ConvertCreateDtoToTeamEntity(TeamCreateDto dto)
    {
        return new Team()
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }

    private TeamDto ConvertEntityToTeamDto(Team team)
    {
        return new TeamDto()
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            Members = team.UserTeams.Select(m => new TeamMemberDto()
            {
                UserId = m.UserId,
                FirstName = m.User.FirstName,
                LastName = m.User.LastName,
                Role = m.User.Role.Name,
                UserName = m.User.UserName,
                JoinedAt = m.JoinedAt,
            }).ToList()
        };
    }
}