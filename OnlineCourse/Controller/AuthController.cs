// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;
using System.Security.Claims;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == null)
                return BadRequest(ApiResponse.Fail("Register", "Đăng ký thất bại"));
            return Ok(ApiResponse.Success("Register", "Đăng ký thành công", result));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(ApiResponse.Fail("Login", "Email hoặc mật khẩu không chính xác", 401));
            return Ok(ApiResponse.Success("Login", "Đăng nhập thành công", result));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse.Fail("ChangePassword", "Không tìm thấy user ID trong token.", 401));
            }

            var success = await _authService.ChangePasswordAsync(userId, dto);
            if (!success)
                return BadRequest(ApiResponse.Fail("ChangePassword", "Mật khẩu cũ không chính xác", 400));
            return Ok(ApiResponse.Success("ChangePassword", "Mật khẩu đã được đổi thành công"));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var success = await _authService.ForgotPasswordAsync(dto);
            if (!success)
                return BadRequest(ApiResponse.Fail("ForgotPassword", "Email không tồn tại"));
            return Ok(ApiResponse.Success("ForgotPassword", "Email đặt lại mật khẩu đã được gửi"));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _authService.ResetPasswordAsync(dto);
            if (!success)
                return BadRequest(ApiResponse.Fail("ResetPassword", "Token không hợp lệ hoặc user không tồn tại"));
            return Ok(ApiResponse.Success("ResetPassword", "Mật khẩu đã được đặt lại thành công"));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            if (result == null)
                return Unauthorized(ApiResponse.Fail("RefreshToken", "Refresh token không hợp lệ", 401));
            return Ok(ApiResponse.Success("RefreshToken", "Token mới đã được cấp", result));
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto dto)
        {
            var success = await _authService.VerifyEmailAsync(dto);
            if (!success)
                return BadRequest(ApiResponse.Fail("VerifyEmail", "Token không hợp lệ hoặc đã hết hạn"));
            return Ok(ApiResponse.Success("VerifyEmail", "Email đã được xác thực"));
        }
    }
}