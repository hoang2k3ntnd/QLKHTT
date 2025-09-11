using AutoMapper;
using OnlineCourse.DTOs;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;
using OnlineCourse.Repositories;

namespace OnlineCourse.Services
{
    /// <summary>
    /// Service cho Role: chỉ nghiệp vụ, không chứa CRUD thô
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return _mapper.Map<RoleDto>(role);
        }
        // lấy danh sách role có phân trang và tìm kiếm
        public async Task<PagedResultDto<RoleDto>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            var roles = await _roleRepository.GetPagedAsync(
                page,
                pageSize,
                r => string.IsNullOrEmpty(search) || r.RoleName.Contains(search)
            );

            var totalCount = await _roleRepository.CountAsync(
                r => string.IsNullOrEmpty(search) || r.RoleName.Contains(search)
            );

            var items = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return new PagedResultDto<RoleDto>(items, totalCount, page, pageSize);
        }


        /// Tạo mới role
        public async Task<RoleDto> CreateAsync(RoleCreateDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleDto>(role);
        }
        /// Cập nhật role
        public async Task<RoleDto> UpdateAsync(RoleUpdateDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(dto.RoleId)
                       ?? throw new KeyNotFoundException("Role not found");

            _mapper.Map(dto, role);
            await _roleRepository.UpdateAsync(role);

            return _mapper.Map<RoleDto>(role);
        }
        /// Xóa role
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _roleRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _roleRepository.DeleteAsync(id);
            return true;
        }
    }
}
