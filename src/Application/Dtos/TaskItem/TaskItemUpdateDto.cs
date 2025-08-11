namespace Application.Dtos.TaskItem;

public class TaskItemUpdateDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}