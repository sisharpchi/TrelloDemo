using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos.Auth;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Persistence.Repositories;

namespace Api.Configurations;

public static class DependecyInjectionsConfiguration
{
    public static void ConfigureDependecies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITeamService, TeamService>();
        //services.AddScoped<IBoardService, BoardService>();


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, UserRoleRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IListColumnRepository, ListColumnRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            

        services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
    }
}