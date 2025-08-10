using Application.Dtos.TaskItem;

namespace Application.Dtos.ListColumn;

public class ListColumnWithTasksDto : ListColumnDto
{
    public ICollection<TaskItemDto> Tasks { get; set; }
}
