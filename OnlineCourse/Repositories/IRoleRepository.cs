using OnlineCourse.Models.Entities;

namespace OnlineCourse.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}
