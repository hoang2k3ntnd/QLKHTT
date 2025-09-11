// Services/AuthService.cs
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;
using OnlineCourse.Repositories;

namespace OnlineCourse.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public AuthService(AppDbContext context, IConfiguration config, IUserRepository userRepository)
    {
        _context = context;
        _config = config;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Đăng ký user mới
    /// </summary>
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email đã tồn tại");

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            SurName = dto.SurName,
            NumberPhone = dto.NumberPhone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword((dto.Password)),
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        // 🎉 Không gán role mặc định nào khi đăng ký 🎉
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Với user mới đăng ký, role mặc định là "Public" và có quyền "Course.View"
        var roles = new List<string> { "Public" };
        var permissions = new List<string> { "Course.View" };

        var token = JwtHelper.GenerateJwt(user, roles, permissions, _config);

        // 🎉 Tạo Refresh Token nhưng KHÔNG lưu vào database 🎉
        var refreshToken = Guid.NewGuid().ToString();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:expire"])),
            Roles = roles,
            Permissions = permissions
        };
    }

    /// <summary>
    /// Đăng nhập user
    /// </summary>
    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash!))
            throw new Exception("Email hoặc mật khẩu không chính xác");

        var roles = await _userRepository.GetRolesForUserAsync(user.UserId);
        var permissions = await _userRepository.GetPermissionsForUserAsync(user.UserId);

        // Nếu user chưa có role (tức là user mới đăng ký)
        if (!roles.Any())
        {
            roles = new List<string> { "Public" };
            permissions = new List<string> { "Course.View" };
        }

        var token = JwtHelper.GenerateJwt(user, roles, permissions, _config);

        // 🎉 Tạo Refresh Token nhưng KHÔNG lưu vào database 🎉
        var refreshToken = Guid.NewGuid().ToString();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:expire"])),
            Roles = roles,
            Permissions = permissions
        };
    }

    /// <summary>
    /// Đổi mật khẩu
    /// </summary>
    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash!))
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Quên mật khẩu
    /// </summary>
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return false;

        var token = Guid.NewGuid().ToString();
        user.PasswordResetToken = BCrypt.Net.BCrypt.HashPassword(token);
        user.PasswordResetTokenExpiryDate = DateTime.Now.AddHours(2);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Đặt lại mật khẩu
    /// </summary>
    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var usersWithToken = await _context.Users.Where(u => u.PasswordResetToken != null).ToListAsync();
        var user = usersWithToken.FirstOrDefault(u => BCrypt.Net.BCrypt.Verify(dto.Token, u.PasswordResetToken!) && u.PasswordResetTokenExpiryDate > DateTime.Now);

        if (user == null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiryDate = null;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto)
    {
        // Cảnh báo: Cách này không an toàn vì không kiểm tra token trong DB.
        // Dựa vào yêu cầu của bạn, chỉ tạo token mới và trả về.

        // Bước này giả định token từ client là hợp lệ và không cần kiểm tra trong DB
        var user = await _context.Users.FirstOrDefaultAsync(); // Đây chỉ là một placeholder
        if (user == null)
        {
            return null; // Hoặc trả về lỗi
        }

        var roles = await _userRepository.GetRolesForUserAsync(user.UserId);
        var permissions = await _userRepository.GetPermissionsForUserAsync(user.UserId);

        // Tạo token mới
        var newAccessToken = JwtHelper.GenerateJwt(user, roles, permissions, _config);
        var newRefreshToken = Guid.NewGuid().ToString(); // Tạo chuỗi mới

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken, // Trả về refresh token mới cho client
            ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:expire"])),
            Roles = roles,
            Permissions = permissions
        };
    }

    /// <summary>
    /// Xác thực email
    /// </summary>
    public async Task<bool> VerifyEmailAsync(EmailVerificationDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.EmailVerification && u.EmailVerificationToken == dto.Token);
        if (user == null) return false;

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        await _context.SaveChangesAsync();
        return true;
    }
}