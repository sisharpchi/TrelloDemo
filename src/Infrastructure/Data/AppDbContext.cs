using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserConfirmer> Confirmers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ListColumn> ListColumns { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
