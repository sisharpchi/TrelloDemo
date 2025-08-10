namespace Application.Dtos.TaskItem;

public class TaskItemCreateDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public long ListColumnId { get; set; }
    public DateTime? DueDate { get; set; }
}