using OnlineCourse.Models.Entities;

namespace OnlineCourse.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<string>> GetRolesForUserAsync(int userId);
        Task<IEnumerable<string>> GetPermissionsForUserAsync(int userId);
        Task AddUserRoleAsync(UserRole userRole);
        Task RemoveUserRoleAsync(int userId, int roleId);
    }
}
