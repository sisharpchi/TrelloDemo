namespace Application.Dtos.Team;

public class TeamDto : TeamUpdateDto
{
    public ICollection<TeamMemberDto>? Members { get; set; }
}
