namespace Domain.Entities;

public class ChatUser
{
    public long Id { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public long ChatId { get; set; }
    public Chat Chat { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}