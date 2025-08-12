namespace Application.Dtos.Comment;

public class CommentCreateDto
{
    public string Content { get; set; }
    public long TaskItemId { get; set; }
    public long? ParentCommentId { get; set; }
}