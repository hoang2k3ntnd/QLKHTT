using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task AddAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Permission permission)
        {
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == name);
        }
    }
}