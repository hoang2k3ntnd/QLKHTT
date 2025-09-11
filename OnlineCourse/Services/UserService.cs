using AutoMapper;
using OnlineCourse.DTOs;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;
using OnlineCourse.Repositories;
namespace OnlineCourse.Services
{
    /// Service cho User: chỉ nghiệp vụ, không chứa CRUD thô
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        // Lấy user theo Id
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = await _userRepository.GetRolesForUserAsync(user.UserId);
            return dto;
        }
        // Lấy danh sách user có phân trang và tìm kiếm
        public async Task<PagedResultDto<UserDto>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            var users = await _userRepository.GetPagedAsync(
                page,
                pageSize,
                u => string.IsNullOrEmpty(search) || u.UserName.Contains(search) || u.Email.Contains(search)
            );

            var totalCount = await _userRepository.CountAsync(
                u => string.IsNullOrEmpty(search) || u.UserName.Contains(search) || u.Email.Contains(search)
            );

            var items = _mapper.Map<IEnumerable<UserDto>>(users);

            // Đúng 4 tham số (items, totalCount, page, pageSize)
            return new PagedResultDto<UserDto>(items, totalCount, page, pageSize);
        }
        // Tạo mới user
        public async Task<UserDto> CreateAsync(UserCreateDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            entity.CreatedAt = DateTime.Now;
            entity.IsActive = true;

            await _userRepository.AddAsync(entity);

            var userDto = _mapper.Map<UserDto>(entity);
            userDto.Roles = await _userRepository.GetRolesForUserAsync(entity.UserId);
            return userDto;
        }
        // Cập nhật user
        public async Task<UserDto> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId)
                       ?? throw new KeyNotFoundException("User not found");

            _mapper.Map(dto, user);
            await _userRepository.UpdateAsync(user);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userRepository.GetRolesForUserAsync(user.UserId);
            return userDto;
        }
        // Xóa user
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }
        // Khóa/Mở khóa user
        public async Task<bool> LockUserAsync(int id, bool IsActive)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = IsActive; // Giả sử có thuộc tính IsLocked trong User
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new KeyNotFoundException("User not found");

            var role = await _roleRepository.GetByIdAsync(roleId)
                       ?? throw new KeyNotFoundException("Role not found");

            await _userRepository.AddUserRoleAsync(new UserRole { UserId = userId, RoleId = roleId });
        }

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            await _userRepository.RemoveUserRoleAsync(userId, roleId);
        }
    }
}
