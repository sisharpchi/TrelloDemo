namespace Domain.Entities;

public class Board
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TeamId { get; set; }
    public Team Team { get; set; }

    public ICollection<ListColumn> Lists { get; set; }
}