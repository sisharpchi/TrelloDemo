using Application.Abstractions.Services;
using Application.Dtos.Message;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Infrastructure.Persistence.SignalR;

public class ChatHub(IMessageService _messageService) : Hub
{
    public async Task SendMessage(MessageCreateDto dto)
    {
        var senderId = long.Parse(Context.UserIdentifier ?? throw new UnauthorizedAccessException());

        var messageId = await _messageService.CreateAsync(dto, senderId);

        var createdMessage = await _messageService.GetByIdAsync(messageId, senderId);

        await Clients.Group(dto.ChatId.ToString()).SendAsync("ReceiveMessage", createdMessage);
    }

    public async Task JoinChat(long chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(long chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}