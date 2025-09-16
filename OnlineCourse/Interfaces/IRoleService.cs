using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto> CreateRoleAsync(RoleCreateDto createDto);
        Task<RoleDto?> UpdateRoleAsync(RoleUpdateDto updateDto);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<bool> AddPermissionAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionAsync(int roleId, int permissionId);
    }
}