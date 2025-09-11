// OnlineCourse/Controllers/UserController.cs
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.User.View)]
        // Get User by Id
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse.Fail("GetUser", "User not found", 404));

            return Ok(ApiResponse.Success("GetUser", "User retrieved successfully", user, 200));
        }

        // Get Paged Users with optional search
        [HttpGet]
        [Authorize(Policy = PermissionConstants.User.View)]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await _userService.GetPagedAsync(page, pageSize, search);
            return Ok(ApiResponse.Success("GetUsers", "Users retrieved successfully", result, 200));
        }

        // Create User
        [HttpPost]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return Ok(ApiResponse.Success("CreateUser", "User created successfully", user, 201));
        }

        // Update User
        [HttpPut]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Update(UserUpdateDto dto)
        {
            var user = await _userService.UpdateAsync(dto);
            if (user == null)
                return NotFound(ApiResponse.Fail("UpdateUser", "User not found", 404));

            return Ok(ApiResponse.Success("UpdateUser", "User updated successfully", user, 200));
        }

        // Soft Delete User
        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse.Fail("DeleteUser", "User not found", 404));

            return Ok(ApiResponse.Success("DeleteUser", "User deleted successfully", null, 200));
        }

        // RBAC: Assign Role to User
        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            try
            {
                await _userService.AssignRoleAsync(userId, roleId);
                var updatedUser = await _userService.GetByIdAsync(userId); // Lấy lại user để trả về dữ liệu mới
                return Ok(ApiResponse.Success("AssignRole", $"Role {roleId} assigned to User {userId}", updatedUser, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse.Fail("AssignRole", ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("AssignRole", $"Internal server error: {ex.Message}", 500));
            }
        }

        // RBAC: Remove Role from User
        [HttpDelete("{userId}/roles/{roleId}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            try
            {
                await _userService.RemoveRoleAsync(userId, roleId);
                var updatedUser = await _userService.GetByIdAsync(userId); // Lấy lại user để trả về dữ liệu mới
                return Ok(ApiResponse.Success("RemoveRole", $"Role {roleId} removed from User {userId}", updatedUser, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse.Fail("RemoveRole", ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("RemoveRole", $"Internal server error: {ex.Message}", 500));
            }
        }
    }
}