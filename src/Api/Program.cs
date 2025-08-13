
using Api.Configurations;
using Api.Endpoints;
using Application.Helpers.SignalR;
using Core.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.ConfigureDataBase();
            builder.ConfigurationJwtAuth();
            builder.ConfigureJwtSettings();
            builder.ConfigureSerilog();
            builder.Services.ConfigureDependecies();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5173", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200",
                        "http://localhost:5173"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });


            ServiceCollectionExtensions.AddSwaggerWithJwt(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowLocalhost5173");
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAuthEndpoints();
            app.MapRoleEndpoints();
            app.MapAdminEndpoints();
            app.MapTeamEndpoints();
            app.MapBoardEndpoints();
            app.MapListColumnEndpoints();
            app.MapTaskEndpoints();
            app.MapChatEndpoints();
            app.MapMessageEndpoints();
            app.MapCommentEndpoints();

            app.MapHub<NotificationHub>("/hubs/notifications");
            app.MapHub<ChatHub>("/hubs/chats");

            app.MapControllers();


            app.Run();
        }
    }
}
