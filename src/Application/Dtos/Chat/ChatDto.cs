namespace Application.Dtos.Chat;

public class ChatDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public long TeamId { get; set; }
}
