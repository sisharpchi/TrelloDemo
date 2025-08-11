using Application.Abstractions.Services;
using Application.Dtos.ListColumn;
using Core.Extensions;

namespace Api.Endpoints;

public static class ListColumnEndpoints
{
    public static void MapListColumnEndpoints(this WebApplication app)
    {
        var listColumnGroup = app.MapGroup("/api/list-columns")
                                 .RequireAuthorization()
                                 .WithTags("ListColumn Management");

        listColumnGroup.MapPost("/", async (ListColumnCreateDto dto, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var id = await service.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/list-columns/{id}", new { id });
        });

        listColumnGroup.MapPut("/", async (ListColumnUpdateDto dto, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.UpdateAsync(dto, userId, ct);
            return Results.NoContent();
        });

        listColumnGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        listColumnGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var dto = await service.GetByIdAsync(id, userId, ct);
            return dto is not null ? Results.Ok(dto) : Results.NotFound();
        });

        listColumnGroup.MapGet("/by-board/{boardId:long}", async (long boardId, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var list = await service.GetByBoardIdAsync(boardId, userId, ct);
            return Results.Ok(list);
        });

        listColumnGroup.MapGet("/{id:long}/with-tasks", async (long id, HttpContext httpContext, IListColumnService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var dto = await service.GetWithTasksAsync(id, userId, ct);
            return dto is not null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
