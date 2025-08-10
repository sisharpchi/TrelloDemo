namespace Application.Dtos.Auth;

public class ConfirmCodeRequest
{
    public string Code { get; set; } = default!;
    public string Email { get; set; } = default!;
}