using Application.Abstractions.Services;
using Application.Dtos.TaskItem;
using Core.Extensions;

namespace Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var taskGroup = app.MapGroup("/api/tasks")
                           .RequireAuthorization()
                           .WithTags("Task Management");

        taskGroup.MapPost("/", async (TaskItemCreateDto dto, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var taskId = await service.CreateAsync(dto, userId, ct);
            return Results.Created($"/api/tasks/{taskId}", new { id = taskId });
        });

        taskGroup.MapPut("/", async (TaskItemUpdateDto dto, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.UpdateAsync(dto, userId, ct);
            return Results.NoContent();
        });

        taskGroup.MapDelete("/{id:long}", async (long id, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.DeleteAsync(id, userId, ct);
            return Results.NoContent();
        });

        taskGroup.MapGet("/{id:long}", async (long id, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var task = await service.GetByIdAsync(id, userId, ct);
            return task != null ? Results.Ok(task) : Results.NotFound();
        });

        taskGroup.MapGet("/by-list-column/{listColumnId:long}", async (long listColumnId, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            var tasks = await service.GetByListColumnIdAsync(listColumnId, userId, ct);
            return Results.Ok(tasks);
        });

        taskGroup.MapPost("/{taskId:long}/move/{targetListId:long}", async (long taskId, long targetListId, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.MoveToListAsync(taskId, targetListId, userId, ct);
            return Results.NoContent();
        });

        taskGroup.MapPost("/{taskId:long}/assign/{assignedUserId:long}", async (long taskId, long assignedUserId, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.AssignUserAsync(taskId, assignedUserId, userId, ct);
            return Results.NoContent();
        });

        taskGroup.MapPost("/{taskId:long}/unassign/{removedUserId:long}", async (long taskId, long removedUserId, HttpContext httpContext, ITaskService service, CancellationToken ct) =>
        {
            var userId = httpContext.User.GetUserId();
            await service.UnassignUserAsync(taskId, removedUserId, userId, ct);
            return Results.NoContent();
        });
    }
}