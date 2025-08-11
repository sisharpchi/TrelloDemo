namespace Application.Dtos.TaskItem;

public class TaskItemDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public long ListColumnId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public long? AssignedUserId { get; set; }
}