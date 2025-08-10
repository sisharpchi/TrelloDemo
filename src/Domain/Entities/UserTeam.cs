using Domain.Enums;

namespace Domain.Entities;

public class UserTeam
{
    public long Id { get; set; }
    public TeamRole Role { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public long UserId { get; set; }
    public User User { get; set; }

    public long TeamId { get; set; }
    public Team Team { get; set; }
}