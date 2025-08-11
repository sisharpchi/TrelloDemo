using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.Chat;
using Application.Dtos.Enums;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ChatService(IChatRepository _chatRepository, ITeamRepository _teamRepository) : IChatService
{
    public async Task<long> CreateAsync(ChatCreateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(dto.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId && (u.Role == (TeamRole)TeamRoleDto.Owner || u.Role == (TeamRole)TeamRoleDto.Admin)))
            throw new UnauthorizedAccessException("Siz ushbu team uchun chat yarata olmaysiz.");

        var chat = new Chat
        {
            Name = dto.Name,
            Bio = dto.Bio,
            TeamId = dto.TeamId
        };

        return await _chatRepository.AddAsync(chat, cancellationToken);
    }

    public async Task UpdateAsync(ChatUpdateDto dto, long thisUserId, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository.GetByIdAsync(dto.Id, cancellationToken)
                   ?? throw new KeyNotFoundException("Chat topilmadi.");

        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId && (u.Role == (TeamRole)TeamRoleDto.Owner || u.Role == (TeamRole)TeamRoleDto.Admin)))
            throw new UnauthorizedAccessException("Siz ushbu chatni o'zgartira olmaysiz.");

        chat.Name = dto.Name;
        chat.Bio = dto.Bio;

        await _chatRepository.UpdateAsync(chat, cancellationToken);
    }

    public async Task DeleteAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new KeyNotFoundException("Chat topilmadi.");

        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId && (u.Role == (TeamRole)TeamRoleDto.Owner || u.Role == (TeamRole)TeamRoleDto.Admin)))
            throw new UnauthorizedAccessException("Siz ushbu chatni o'chira olmaysiz.");

        await _chatRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<ChatDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository.GetByIdAsync(id, cancellationToken);
        if (chat is null)
            return null;

        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz ushbu chatni ko'ra olmaysiz.");

        return new ChatDto
        {
            Id = chat.Id,
            Name = chat.Name,
            Bio = chat.Bio,
            CreatedAt = chat.CreatedAt,
            TeamId = chat.TeamId
        };
    }

    public async Task<ChatDto?> GetByTeamIdAsync(long teamId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(teamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz ushbu team chatini ko'ra olmaysiz.");

        var chat = await _chatRepository.GetByTeamIdAsync(teamId, cancellationToken);
        if (chat is null)
            return null;

        return new ChatDto
        {
            Id = chat.Id,
            Name = chat.Name,
            Bio = chat.Bio,
            CreatedAt = chat.CreatedAt,
            TeamId = chat.TeamId
        };
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _chatRepository.ExistsAsync(id, cancellationToken);
    }
}