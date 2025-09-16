using OnlineCourse.Models.Entities;

namespace OnlineCourse.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(int id);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(Permission permission);
        Task<Permission?> GetByNameAsync(string name);
    }
}