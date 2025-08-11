using Application.Abstractions.Services;
using Application.Dtos.Board;
using Core.Extensions;

namespace Api.Endpoints;

public static class BoardEndpoints
{
    public static void MapBoardEndpoints(this WebApplication app)
    {
        var boardGroup = app.MapGroup("/api/boards")
                            .RequireAuthorization()
                            .WithTags("Board Management");

        boardGroup.MapPost("/", async (BoardCreateDto dto, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var boardId = await boardService.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/boards/{boardId}", new { id = boardId });
        });

        boardGroup.MapPut("/", async (BoardUpdateDto dto, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await boardService.UpdateAsync(dto, userId, ct);
            return Results.NoContent();
        });

        boardGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await boardService.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        boardGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var board = await boardService.GetByIdAsync(id, userId, ct);
            return board is not null ? Results.Ok(board) : Results.NotFound();
        });

        boardGroup.MapGet("/by-team/{teamId:long}", async (long teamId, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var boards = await boardService.GetByTeamIdAsync(teamId, userId, ct);
            return Results.Ok(boards);
        });

        boardGroup.MapGet("/{id:long}/with-lists", async (long id, HttpContext httpContext, IBoardService boardService, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var board = await boardService.GetWithListsAsync(id, userId, ct);
            return board is not null ? Results.Ok(board) : Results.NotFound();
        });
    }
}
