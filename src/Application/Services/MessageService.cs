using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.Message;
using Application.Helpers.SignalR;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class MessageService(IMessageRepository _messageRepository, IChatRepository _chatRepository, ITeamRepository _teamRepository, IHubContext<ChatHub> _hubContext) : IMessageService
{
    public async Task<long> CreateAsync(MessageCreateDto dto, long senderId, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository.GetByIdAsync(dto.ChatId, cancellationToken)
                   ?? throw new KeyNotFoundException("Chat topilmadi.");

        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == senderId))
            throw new UnauthorizedAccessException("Siz ushbu chatga xabar yubora olmaysiz.");

        var message = new Message
        {
            Content = dto.Content,
            ChatId = dto.ChatId,
            SenderId = senderId,
            ReplyMessageId = dto.ReplyMessageId
        };

        message.Id = await _messageRepository.AddAsync(message, cancellationToken);

        var result = MapToDto(message);

        await _hubContext.Clients.Group($"chat_{dto.ChatId}")
            .SendAsync("MessageCreated", result, cancellationToken);

        return message.Id;
    }

    public async Task<MessageDto> UpdateAsync(MessageUpdateDto dto, long senderId, CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetByIdAsync(dto.Id, cancellationToken)
                      ?? throw new KeyNotFoundException("Xabar topilmadi.");

        if (message.SenderId != senderId)
            throw new UnauthorizedAccessException("Faqat o'zingiz yozgan xabarni tahrirlashingiz mumkin.");

        message.Content = dto.Content;

        await _messageRepository.UpdateAsync(message, cancellationToken);

        var result = MapToDto(message);

        await _hubContext.Clients.Group($"chat_{message.ChatId}")
            .SendAsync("MessageUpdated", result, cancellationToken);

        return result;
    }

    public async Task DeleteAsync(long id, long senderId, CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetByIdAsync(id, cancellationToken)
                      ?? throw new KeyNotFoundException("Xabar topilmadi.");

        if (message.SenderId != senderId)
            throw new UnauthorizedAccessException("Faqat o'zingiz yozgan xabarni o'chirishingiz mumkin.");

        await _messageRepository.DeleteAsync(id, cancellationToken);

        await _hubContext.Clients.Group($"chat_{message.ChatId}")
            .SendAsync("MessageDeleted", id, cancellationToken);
    }

    public async Task<MessageDto?> GetByIdAsync(long id, long thisUserId, CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetByIdAsync(id, cancellationToken);
        if (message is null) return null;

        var chat = await _chatRepository.GetByIdAsync(message.ChatId, cancellationToken);
        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);

        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz ushbu xabarni ko‘ra olmaysiz.");

        return new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            SentAt = message.SentAt,
            ChatId = message.ChatId,
            SenderId = message.SenderId,
            ReplyMessageId = message.ReplyMessageId
        };
    }

    public async Task<ICollection<MessageDto>> GetByChatIdAsync(long chatId, long thisUserId, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId, cancellationToken)
                   ?? throw new KeyNotFoundException("Chat topilmadi.");

        var team = await _teamRepository.GetByIdAsync(chat.TeamId, cancellationToken);
        if (team is null || !team.UserTeams.Any(u => u.UserId == thisUserId))
            throw new UnauthorizedAccessException("Siz ushbu chatni ko‘ra olmaysiz.");

        var messages = await _messageRepository.GetByChatIdAsync(chatId, cancellationToken);

        return messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            SentAt = m.SentAt,
            ChatId = m.ChatId,
            SenderId = m.SenderId,
            ReplyMessageId = m.ReplyMessageId
        }).ToList();
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return _messageRepository.ExistsAsync(id, cancellationToken);
    }

    private static MessageDto MapToDto(Message message) =>
       new MessageDto
       {
           Id = message.Id,
           Content = message.Content,
           SentAt = message.SentAt,
           ChatId = message.ChatId,
           SenderId = message.SenderId,
           ReplyMessageId = message.ReplyMessageId
       };
}
