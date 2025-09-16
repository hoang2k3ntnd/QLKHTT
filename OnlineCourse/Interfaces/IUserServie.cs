using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto> CreateUserAsync(UserCreateDto createDto);
        Task<UserDto?> UpdateUserAsync(UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
    }
}