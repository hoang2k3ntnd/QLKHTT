using OnlineCourse.Models.Entities;

namespace OnlineCourse.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role?> GetByNameAsync(string roleName);
        Task AddPermissionToRoleAsync(RolePermission rolePermission);
        Task RemovePermissionFromRoleAsync(RolePermission rolePermission);
        Task<RolePermission?> GetRolePermissionAsync(int roleId, int permissionId);
        Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId);
    }
}