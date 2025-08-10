using Application.Dtos.Auth;

namespace Application.Abstractions.Services;

public interface IRoleService
{
    Task<ICollection<UserGetDto>> GetAllUsersByRoleAsync(string role);
    Task<List<RoleGetDto>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
}