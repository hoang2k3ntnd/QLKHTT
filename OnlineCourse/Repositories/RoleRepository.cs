using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        //private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context) : base(context)
        {
            //_context = context;
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}
