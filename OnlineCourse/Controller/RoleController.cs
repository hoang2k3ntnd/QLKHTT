// OnlineCourse/Controllers/RoleController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Constants; // Re-use the existing constants
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require authentication by default
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponse.Success("GetAllRoles", "Roles retrieved successfully", roles, 200));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound(ApiResponse.Fail("GetRoleById", "Role not found", 404));
            }
            return Ok(ApiResponse.Success("GetRoleById", "Role retrieved successfully", role, 200));
        }

        [HttpPost]
        [Authorize(Policy = PermissionConstants.Role.Create)]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto createDto)
        {
            var createdRole = await _roleService.CreateRoleAsync(createDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, ApiResponse.Success("CreateRole", "Role created successfully", createdRole, 201));
        }

        [HttpPut]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDto updateDto)
        {
            var updatedRole = await _roleService.UpdateRoleAsync(updateDto);
            if (updatedRole == null)
            {
                return NotFound(ApiResponse.Fail("UpdateRole", "Role not found", 404));
            }
            return Ok(ApiResponse.Success("UpdateRole", "Role updated successfully", updatedRole, 200));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Delete)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success)
            {
                return NotFound(ApiResponse.Fail("DeleteRole", "Role not found", 404));
            }
            return Ok(ApiResponse.Success("DeleteRole", "Role deleted successfully", null, 200));
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> AddPermissionToRole(int roleId, int permissionId)
        {
            var success = await _roleService.AddPermissionAsync(roleId, permissionId);
            if (!success)
            {
                return BadRequest(ApiResponse.Fail("AddPermissionToRole", "Failed to add permission. Either the role/permission does not exist or the permission is already assigned.", 400));
            }
            return Ok(ApiResponse.Success("AddPermissionToRole", "Permission added to role successfully", null, 200));
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> RemovePermissionFromRole(int roleId, int permissionId)
        {
            var success = await _roleService.RemovePermissionAsync(roleId, permissionId);
            if (!success)
            {
                return NotFound(ApiResponse.Fail("RemovePermissionFromRole", "Failed to remove permission. Relationship not found.", 404));
            }
            return Ok(ApiResponse.Success("RemovePermissionFromRole", "Permission removed from role successfully", null, 200));
        }
    }
}