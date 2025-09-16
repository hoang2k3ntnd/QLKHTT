using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.DTOs
{
    // --------------------
    // Register & Login
    // --------------------
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        public string? SurName { get; set; }

        [Phone]
        [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 characters")]
        public string? NumberPhone { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; } // Thuộc tính đã được thêm
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<string> Permissions { get; set; } = new List<string>();

    }

    // --------------------
    // Password management
    // --------------------
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }

    // --------------------
    // Refresh Token
    // --------------------
    // DTO này có thể được loại bỏ nếu bạn sử dụng cookie
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    // --------------------
    // Email verification
    // --------------------
    public class EmailVerificationDto
    {
        [Required]
        [EmailAddress]
        public string EmailVerification { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}