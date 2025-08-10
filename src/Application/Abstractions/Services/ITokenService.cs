using Application.Dtos.Auth;
using System.Security.Claims;

namespace Application.Abstractions.Services;

public interface ITokenService
{
    public string GenerateToken(UserGetDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}