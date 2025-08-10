namespace Application.Dtos.Board;

public class BoardUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}