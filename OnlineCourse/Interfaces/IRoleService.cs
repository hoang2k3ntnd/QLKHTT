using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetByIdAsync(int id);

        /// <summary>
        /// Lấy danh sách role có phân trang + tìm kiếm
        /// </summary>
        Task<PagedResultDto<RoleDto>> GetPagedAsync(int page, int pageSize, string? search = null);

        Task<RoleDto> CreateAsync(RoleCreateDto dto);
        Task<RoleDto> UpdateAsync(RoleUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
