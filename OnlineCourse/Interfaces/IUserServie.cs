using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    /// Service User: chỉ chứa business logic, không truy cập DB trực tiếp.
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserCreateDto dto);
        Task<UserDto> UpdateAsync(UserUpdateDto dto);
        Task<bool> LockUserAsync(int id, bool isActive);
        Task<UserDto?> GetByIdAsync(int id);
        Task<PagedResultDto<UserDto>> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<bool> DeleteAsync(int id);

        // RBAC
        Task AssignRoleAsync(int userId, int roleId);
        Task RemoveRoleAsync(int userId, int roleId);
    }
}
