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
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) =>
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));

        // ------------------------------
        // GET: api/role
        // ------------------------------
        [HttpGet]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponse.Success("GetAllRoles", "Roles retrieved successfully", roles, 200));
        }

        // ------------------------------
        // GET: api/role/{id}
        // ------------------------------
        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return Ok(ApiResponse.Fail("GetRoleById", "Role not found", 404));

            return Ok(ApiResponse.Success("GetRoleById", "Role retrieved successfully", role, 200));
        }

        // ------------------------------
        // POST: api/role
        // ------------------------------
        [HttpPost]
        [Authorize(Policy = PermissionConstants.Role.Create)]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
        {
            var role = await _roleService.CreateRoleAsync(dto);
            return Ok(ApiResponse.Success("CreateRole", "Role created successfully", role, 201));
        }

        // ------------------------------
        // PUT: api/role
        // ------------------------------
        [HttpPut]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDto dto)
        {
            var role = await _roleService.UpdateRoleAsync(dto);
            if (role == null)
                return Ok(ApiResponse.Fail("UpdateRole", "Role not found", 404));

            return Ok(ApiResponse.Success("UpdateRole", "Role updated successfully", role, 200));
        }

        // ------------------------------
        // DELETE: api/role/{id}
        // ------------------------------
        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Delete)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success)
                return Ok(ApiResponse.Fail("DeleteRole", "Role not found", 404));

            return Ok(ApiResponse.Success("DeleteRole", "Role deleted successfully", null, 200));
        }

        // ------------------------------
        // POST: api/role/{roleId}/permissions/{permissionId}
        // ------------------------------
        [HttpPost("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> AddPermissionToRole(int roleId, int permissionId)
        {
            var success = await _roleService.AddPermissionAsync(roleId, permissionId);
            if (!success)
                return Ok(ApiResponse.Fail("AddPermissionToRole",
                    "Failed to add permission. Role/Permission not found or already assigned.", 400));

            return Ok(ApiResponse.Success("AddPermissionToRole",
                "Permission added to role successfully", null, 200));
        }

        // ------------------------------
        // DELETE: api/role/{roleId}/permissions/{permissionId}
        // ------------------------------
        [HttpDelete("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> RemovePermissionFromRole(int roleId, int permissionId)
        {
            var success = await _roleService.RemovePermissionAsync(roleId, permissionId);
            if (!success)
                return Ok(ApiResponse.Fail("RemovePermissionFromRole",
                    "Failed to remove permission. Relationship not found.", 404));

            return Ok(ApiResponse.Success("RemovePermissionFromRole",
                "Permission removed from role successfully", null, 200));
        }
    }
}
