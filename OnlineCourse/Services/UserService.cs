using OnlineCourse.DTOs;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                SurName = u.SurName,
                NumberPhone = u.NumberPhone,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,

                Roles = u.UserRoles?.Select(ur => ur.Role.RoleName) ?? new List<string>()
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                SurName = user.SurName,
                NumberPhone = user.NumberPhone,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,

                Roles = user.UserRoles?.Select(ur => ur.Role.RoleName) ?? new List<string>()
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto createDto)
        {
            var user = new User
            {
                UserName = createDto.UserName,
                Email = createDto.Email,
                SurName = createDto.SurName,
                NumberPhone = createDto.NumberPhone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password),
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsVerified = false,

            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                SurName = user.SurName,
                NumberPhone = user.NumberPhone,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,

            };
        }

        public async Task<UserDto?> UpdateUserAsync(UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(updateDto.UserId);
            if (user == null)
            {
                return null;
            }

            if (updateDto.UserName != null) user.UserName = updateDto.UserName;
            if (updateDto.Email != null) user.Email = updateDto.Email;
            if (updateDto.SurName != null) user.SurName = updateDto.SurName;
            if (updateDto.NumberPhone != null) user.NumberPhone = updateDto.NumberPhone;

            await _userRepository.UpdateAsync(user);

            return await GetUserByIdAsync(user.UserId);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var userRoleExists = await _userRepository.GetUserRoleAsync(userId, roleId) != null;

            if (user == null || userRoleExists)
            {
                return false;
            }

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            await _userRepository.AddUserRoleAsync(userRole);
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _userRepository.GetUserRoleAsync(userId, roleId);
            if (userRole == null)
            {
                return false;
            }

            await _userRepository.RemoveUserRoleAsync(userRole);
            return true;
        }
    }
}