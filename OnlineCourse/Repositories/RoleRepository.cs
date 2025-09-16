using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task AddPermissionToRoleAsync(RolePermission rolePermission)
        {
            await _context.RolePermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionFromRoleAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task<RolePermission?> GetRolePermissionAsync(int roleId, int permissionId)
        {
            return await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }
    }
}