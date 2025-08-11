using Application.Abstractions.Services;
using Application.Dtos.Chat;
using Core.Extensions;

namespace Api.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this WebApplication app)
    {
        var chatGroup = app.MapGroup("/api/chats")
                           .RequireAuthorization()
                           .WithTags("Chat Management");

        chatGroup.MapPost("/", async (ChatCreateDto dto, HttpContext httpContext, IChatService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var chatId = await service.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/chats/{chatId}", new { id = chatId });
        });

        chatGroup.MapPut("/", async (ChatUpdateDto dto, HttpContext httpContext, IChatService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.UpdateAsync(dto, userId, ct);
            return Results.NoContent();
        });

        chatGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, IChatService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        chatGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, IChatService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var chat = await service.GetByIdAsync(id, userId, ct);
            return chat != null ? Results.Ok(chat) : Results.NotFound();
        });

        chatGroup.MapGet("/by-team/{teamId:long}", async (long teamId, HttpContext httpContext, IChatService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var chats = await service.GetByTeamIdAsync(teamId, userId, ct);
            return Results.Ok(chats);
        });
    }
}