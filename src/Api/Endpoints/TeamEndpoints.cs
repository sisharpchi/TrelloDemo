using Application.Abstractions.Services;
using Application.Dtos.Team;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Api.Endpoints;

public static class TeamEndpoints
{
    public static void MapTeamEndpoints(this WebApplication app)
    {
        var teamGroup = app.MapGroup("/api/team")
                           .WithTags("Team Management")
                           .RequireAuthorization();

        teamGroup.MapPost("/",
        async (TeamCreateDto dto, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            var teamId = await teamService.CreateAsync(dto, thisUserId);
            return Results.Ok(new { success = true, id = teamId });
        })
        .WithName("CreateTeam");

        teamGroup.MapPut("/", 
        async (TeamUpdateDto dto, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            await teamService.UpdateAsync(dto, thisUserId);
            return Results.Ok(new { success = true });
        })
        .WithName("UpdateTeam");

        teamGroup.MapDelete("/{id:long}",
        async (long id, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            await teamService.DeleteAsync(id, thisUserId);
            return Results.Ok(new { success = true });
        })
        .WithName("DeleteTeam");

        teamGroup.MapGet("/{id:long}", 
        async (long id, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            var team = await teamService.GetByIdAsync(id, thisUserId);
            return team is not null
                ? Results.Ok(new { success = true, data = team })
                : Results.NotFound(new { success = false, message = "Team topilmadi" });
        })
        .WithName("GetTeamById");

        teamGroup.MapGet("",
        async (HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            var teams = await teamService.GetByUserIdAsync(thisUserId);
            return Results.Ok(new { success = true, data = teams });
        })
        .WithName("GetTeamsByUserId");

        teamGroup.MapPost("/{teamId:long}/members/add/{addedUserId:long}", 
        async (long teamId, long addedUserId, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            await teamService.AddMemberAsync(teamId, addedUserId, thisUserId);
            return Results.Ok(new { success = true });
        })
        .WithName("AddTeamMember");

        teamGroup.MapDelete("/{teamId:long}/members/remove/{removedUserId:long}",
        async (long teamId, long removedUserId, HttpContext context, ITeamService teamService) =>
        {
            var thisUserId = context.User.GetUserId();
            await teamService.RemoveMemberAsync(teamId, removedUserId, thisUserId);
            return Results.Ok(new { success = true });
        })
        .WithName("RemoveTeamMember");
    }
}