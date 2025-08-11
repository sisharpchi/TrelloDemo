using Application.Abstractions.Services;
using Application.Dtos.Message;
using Core.Extensions;

namespace Api.Endpoints;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        var messageGroup = app.MapGroup("/api/messages")
                              .RequireAuthorization()
                              .WithTags("Message Management");

        messageGroup.MapPost("/", async (MessageCreateDto dto, HttpContext httpContext, IMessageService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var messageId = await service.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/messages/{messageId}", new { id = messageId });
        });

        messageGroup.MapPut("/", async (MessageUpdateDto dto, HttpContext httpContext, IMessageService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.UpdateAsync(dto, userId, ct);
            return Results.NoContent();
        });

        messageGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, IMessageService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        messageGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, IMessageService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var message = await service.GetByIdAsync(id, userId, ct);
            return message != null ? Results.Ok(message) : Results.NotFound();
        });

        messageGroup.MapGet("/by-chat/{chatId:long}", async (long chatId, HttpContext httpContext, IMessageService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var messages = await service.GetByChatIdAsync(chatId, userId, ct);
            return Results.Ok(messages);
        });
    }
}