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

        public PermissionController(IPermissionService permissionService) =>
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

        // ------------------------------
        // GET: api/permission
        // ------------------------------
        [HttpGet]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(ApiResponse.Success("GetAllPermissions", "Permissions retrieved successfully", permissions, 200));
        }

        // ------------------------------
        // GET: api/permission/{id}
        // ------------------------------
        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.Role.View)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
                return Ok(ApiResponse.Fail("GetPermissionById", "Permission not found", 404));

            return Ok(ApiResponse.Success("GetPermissionById", "Permission retrieved successfully", permission, 200));
        }

        // ------------------------------
        // POST: api/permission
        // ------------------------------
        [HttpPost]
        [Authorize(Policy = PermissionConstants.Role.Create)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionCreateDto dto)
        {
            var permission = await _permissionService.CreatePermissionAsync(dto);
            return Ok(ApiResponse.Success("CreatePermission", "Permission created successfully", permission, 201));
        }

        // ------------------------------
        // PUT: api/permission
        // ------------------------------
        [HttpPut]
        [Authorize(Policy = PermissionConstants.Role.Edit)]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionUpdateDto dto)
        {
            var permission = await _permissionService.UpdatePermissionAsync(dto);
            if (permission == null)
                return Ok(ApiResponse.Fail("UpdatePermission", "Permission not found", 404));

            return Ok(ApiResponse.Success("UpdatePermission", "Permission updated successfully", permission, 200));
        }

        // ------------------------------
        // DELETE: api/permission/{id}
        // ------------------------------
        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.Role.Delete)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var success = await _permissionService.DeletePermissionAsync(id);
            if (!success)
                return Ok(ApiResponse.Fail("DeletePermission", "Permission not found", 404));

            return Ok(ApiResponse.Success("DeletePermission", "Permission deleted successfully", null, 200));
        }
    }
}
