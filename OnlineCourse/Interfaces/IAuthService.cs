using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
        Task<bool> VerifyEmailAsync(EmailVerificationDto dto);

        //Task<bool> SendVerificationEmailAsync(string email);
    }
}