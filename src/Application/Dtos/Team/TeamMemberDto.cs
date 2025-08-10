namespace Application.Dtos.Team;

public class TeamMemberDto
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public DateTime JoinedAt { get; set; }
}
