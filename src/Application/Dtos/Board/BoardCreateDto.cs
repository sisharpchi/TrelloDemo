namespace Application.Dtos.Board;

public class BoardCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public long TeamId { get; set; }
}