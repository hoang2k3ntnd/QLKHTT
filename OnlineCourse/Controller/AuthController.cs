using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;
using System.Security.Claims;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == null)
            {
                return BadRequest(ApiResponse.Fail("Register", "Email đã tồn tại."));
            }
            return Ok(ApiResponse.Success("Register", "Đăng ký thành công.", result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
            {
                return Unauthorized(ApiResponse.Fail("Login", "Email hoặc mật khẩu không hợp lệ, hoặc tài khoản chưa được xác thực."));
            }
            return Ok(ApiResponse.Success("Login", "Đăng nhập thành công.", result));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            // Lấy UserId từ token của người dùng đã xác thực
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(ApiResponse.Fail("ChangePassword", "Người dùng không hợp lệ."));
            }

            var result = await _authService.ChangePasswordAsync(userId, dto);
            if (!result)
            {
                return BadRequest(ApiResponse.Fail("ChangePassword", "Mật khẩu cũ không đúng."));
            }
            return Ok(ApiResponse.Success("ChangePassword", "Thay đổi mật khẩu thành công."));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto);
            // Luôn trả về OK để tránh bị tấn công email enumeration
            return Ok(ApiResponse.Success("ForgotPassword", "Nếu email của bạn tồn tại trong hệ thống, một liên kết khôi phục mật khẩu sẽ được gửi đến bạn."));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            if (!result)
            {
                return BadRequest(ApiResponse.Fail("ResetPassword", "Token không hợp lệ hoặc đã hết hạn."));
            }
            return Ok(ApiResponse.Success("ResetPassword", "Đặt lại mật khẩu thành công."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            if (result == null)
            {
                return Unauthorized(ApiResponse.Fail("RefreshToken", "Refresh Token không hợp lệ hoặc đã hết hạn."));
            }
            return Ok(ApiResponse.Success("RefreshToken", "Token đã được làm mới thành công.", result));
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto dto)
        {
            var result = await _authService.VerifyEmailAsync(dto);
            if (!result)
            {
                return BadRequest(ApiResponse.Fail("VerifyEmail", "Mã xác thực không hợp lệ."));
            }
            return Ok(ApiResponse.Success("VerifyEmail", "Xác thực email thành công."));
        }
    }
}