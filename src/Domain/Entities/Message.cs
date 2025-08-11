namespace Domain.Entities;

public class Message
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public long ChatId { get; set; }
    public Chat Chat { get; set; }

    public long SenderId { get; set; }
    public User Sender { get; set; }

    public long? ReplyMessageId { get; set; }
    public Message? ReplyMessage { get; set; }

    public ICollection<Message> ReplyMessages { get; set; }
}