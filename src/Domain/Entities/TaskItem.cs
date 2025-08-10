using Domain.Enums;
using System.Net.Mail;
using System.Xml.Linq;

namespace Domain.Entities;

public class TaskItem
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskItemStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long ListColumnId { get; set; }
    public ListColumn ListColumn { get; set; }

    public long? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}
