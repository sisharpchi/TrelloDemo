namespace Domain.Entities;

public class Chat
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Bio {  get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TeamId { get; set; }
    public Team Team { get; set; } = default!;

    public ICollection<ChatUser> ChatUsers { get; set; }
    public ICollection<Message> Messages { get; set; }
}