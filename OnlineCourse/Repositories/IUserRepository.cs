using OnlineCourse.Models.Entities;

namespace OnlineCourse.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<UserRole?> GetUserRoleAsync(int userId, int roleId);
        Task AddUserRoleAsync(UserRole userRole);
        Task RemoveUserRoleAsync(UserRole userRole);
    }
}