// OnlineCourse/Controllers/PermissionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Constants;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bảo vệ toàn bộ controller
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(ApiResponse.Success("GetAllPermissions", "Permissions retrieved successfully", permissions, 200));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return NotFound(ApiResponse.Fail("GetPermissionById", "Permission not found", 404));
            }
            return Ok(ApiResponse.Success("GetPermissionById", "Permission retrieved successfully", permission, 200));
        }

        [HttpPost]
        [Authorize(Policy = PermissionConstants.Role.Create)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionCreateDto dto)
        {
            var createdPermission = await _permissionService.CreatePermissionAsync(dto);
            return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.PermissionId }, ApiResponse.Success("CreatePermission", "Permission created successfully", createdPermission, 201));
        }

        [HttpPut]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionUpdateDto dto)
        {
            var updatedPermission = await _permissionService.UpdatePermissionAsync(dto);
            if (updatedPermission == null)
            {
                return NotFound(ApiResponse.Fail("UpdatePermission", "Permission not found", 404));
            }
            return Ok(ApiResponse.Success("UpdatePermission", "Permission updated successfully", updatedPermission, 200));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Delete)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var success = await _permissionService.DeletePermissionAsync(id);
            if (!success)
            {
                return NotFound(ApiResponse.Fail("DeletePermission", "Permission not found", 404));
            }
            return Ok(ApiResponse.Success("DeletePermission", "Permission deleted successfully", null, 200));
        }
    }
}