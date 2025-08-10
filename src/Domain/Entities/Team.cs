using System;

namespace Domain.Entities;

public class Team
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long ChatId { get; set; }
    public Chat Chat { get; set; }

    public ICollection<UserTeam> UserTeams { get; set; }
    public ICollection<Board> Boards { get; set; }
}