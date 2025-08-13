using Application.Abstractions.Services;
using Application.Dtos.Comment;
using Core.Extensions;
using System.Security.Claims;

namespace Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var commentGroup = app.MapGroup("/api/comments")
                              .RequireAuthorization()
                              .WithTags("Comment Management");

        commentGroup.MapPost("/", async (CommentCreateDto dto, HttpContext httpContext, ICommentService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var comment = await service.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/comments/{comment.Id}", comment);
        });

        commentGroup.MapPut("/", async (CommentUpdateDto dto, HttpContext httpContext, ICommentService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var updated = await service.UpdateAsync(dto, userId, ct);
            return Results.Ok(updated);
        });

        commentGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, ICommentService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        commentGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, ICommentService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var comment = await service.GetByIdAsync(id, userId, ct);
            return comment is not null ? Results.Ok(comment) : Results.NotFound();
        });

        commentGroup.MapGet("/by-task/{taskId:long}", async (long taskId, HttpContext httpContext, ICommentService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var comments = await service.GetByTaskIdAsync(taskId, userId, ct);
            return Results.Ok(comments);
        });
    }
}