namespace Application.Dtos.Comment;

public class CommentResponseDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public long TaskItemId { get; set; }
    public long AuthorId { get; set; }
    public string AuthorName { get; set; }
    public long? ParentCommentId { get; set; }
    public ICollection<CommentResponseDto> Replies { get; set; }
}
