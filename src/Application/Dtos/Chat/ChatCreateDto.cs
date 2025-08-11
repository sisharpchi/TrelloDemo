namespace Application.Dtos.Chat;

public class ChatCreateDto
{
    public string Name { get; set; }
    public string? Bio { get; set; }
    public long TeamId { get; set; }
}