using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Constants;
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Áp dụng Authorization mặc định cho toàn bộ controller
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Lấy danh sách Role có phân trang và tìm kiếm
        [HttpGet]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await _roleService.GetPagedAsync(page, pageSize, search);
            return Ok(ApiResponse.Success("GetRoles", "Roles retrieved successfully", result, 200));
        }

        // Lấy Role theo Id
        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound(ApiResponse.Fail("GetRole", "Role not found", 404));

            return Ok(ApiResponse.Success("GetRole", "Role retrieved successfully", role, 200));
        }

        // Tạo mới Role
        [HttpPost]
        [Authorize(Policy = PermissionConstants.Role.Create)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            var newRole = await _roleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newRole.RoleId }, ApiResponse.Success("CreateRole", "Role created successfully", newRole, 201));
        }

        // Cập nhật Role
        [HttpPut("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto dto)
        {
            if (id != dto.RoleId)
            {
                return BadRequest(ApiResponse.Fail("UpdateRole", "Role ID in URL and body do not match", 400));
            }

            try
            {
                var updatedRole = await _roleService.UpdateAsync(dto);
                return Ok(ApiResponse.Success("UpdateRole", "Role updated successfully", updatedRole, 200));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse.Fail("UpdateRole", "Role not found", 404));
            }
        }

        // Xóa Role
        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _roleService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse.Fail("DeleteRole", "Role not found", 404));

            return NoContent();
        }
    }
}