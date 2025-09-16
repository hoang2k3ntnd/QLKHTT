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

        public UserController(IUserService userService) =>
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        // ------------------------------
        // GET: api/user/{id}
        // ------------------------------
        [HttpGet("{id}")]
        [Authorize(Policy = PermissionConstants.User.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return Ok(ApiResponse.Fail("GetUser", "User not found", 404));

            return Ok(ApiResponse.Success("GetUser", "User retrieved successfully", user, 200));
        }

        // ------------------------------
        // POST: api/user
        // ------------------------------
        [HttpPost]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var user = await _userService.CreateUserAsync(dto);
            return Ok(ApiResponse.Success("CreateUser", "User created successfully", user, 201));
        }

        // ------------------------------
        // PUT: api/user
        // ------------------------------
        [HttpPut]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Update(UserUpdateDto dto)
        {
            var user = await _userService.UpdateUserAsync(dto);
            if (user == null)
                return Ok(ApiResponse.Fail("UpdateUser", "User not found", 404));

            return Ok(ApiResponse.Success("UpdateUser", "User updated successfully", user, 200));
        }

        // ------------------------------
        // DELETE: api/user/{id}
        // ------------------------------
        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return Ok(ApiResponse.Fail("DeleteUser", "User not found", 404));

            return Ok(ApiResponse.Success("DeleteUser", "User deleted successfully", null, 200));
        }

        // ------------------------------
        // POST: api/user/{userId}/roles/{roleId}
        // ------------------------------
        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            try
            {
                await _userService.AssignRoleAsync(userId, roleId);
                var updatedUser = await _userService.GetUserByIdAsync(userId);

                return Ok(ApiResponse.Success("AssignRole",
                    $"Role {roleId} assigned to User {userId}", updatedUser, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return Ok(ApiResponse.Fail("AssignRole", ex.Message, 404));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse.Fail("AssignRole", $"Internal server error: {ex.Message}", 500));
            }
        }

        // ------------------------------
        // DELETE: api/user/{userId}/roles/{roleId}
        // ------------------------------
        [HttpDelete("{userId}/roles/{roleId}")]
        [Authorize(Policy = PermissionConstants.User.Manage)]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            try
            {
                await _userService.RemoveRoleAsync(userId, roleId);
                var updatedUser = await _userService.GetUserByIdAsync(userId);

                return Ok(ApiResponse.Success("RemoveRole",
                    $"Role {roleId} removed from User {userId}", updatedUser, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return Ok(ApiResponse.Fail("RemoveRole", ex.Message, 404));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse.Fail("RemoveRole", $"Internal server error: {ex.Message}", 500));
            }
        }
    }
}
