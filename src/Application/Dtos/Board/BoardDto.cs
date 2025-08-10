namespace Application.Dtos.Board;

public class BoardDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public long TeamId { get; set; }
    public ICollection<ListColumnDto>? Lists { get; set; }
}
