namespace Domain.Entities;

public class ListColumn
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }

    public long BoardId { get; set; }
    public Board Board { get; set; }

    public ICollection<TaskItem> Tasks { get; set; }
}