namespace Domain.Entities;

public class Attachment
{
    public long Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public long TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; }
}