namespace Application.Dtos.Message;

public class MessageCreateDto
{
    public string Content { get; set; }
    public long ChatId { get; set; }
    public long? ReplyMessageId { get; set; }
}
