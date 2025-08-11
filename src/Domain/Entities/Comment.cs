namespace Domain.Entities;

public class Comment
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; }

    public long AuthorId { get; set; }
    public User Author { get; set; }

    public long? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public ICollection<Comment> ReplyComments { get; set; }
}