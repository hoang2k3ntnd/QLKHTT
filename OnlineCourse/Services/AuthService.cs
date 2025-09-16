using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.DTOs;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService emailService;

        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            this.emailService = emailService;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (existingUser)
            {
                return null;
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                SurName = dto.SurName,
                NumberPhone = dto.NumberPhone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.Now,
                IsActive = true,

            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            var userRoles = new List<string>();
            var userPermissions = new List<string>();

            if (defaultRole != null)
            {
                _context.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = defaultRole.RoleId });
                await _context.SaveChangesAsync();
                userRoles.Add(defaultRole.RoleName);
            }

            var accessToken = JwtHelper.GenerateJwt(user, userRoles, userPermissions, _configuration);
            var refreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:expire"])),
                RefreshTokenExpiresAt = user.RefreshTokenExpiryTime,
                Roles = userRoles,
                Permissions = userPermissions,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles!)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            var roles = user.UserRoles?.Select(ur => ur.Role.RoleName) ?? Enumerable.Empty<string>();
            var permissions = user.UserRoles?.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.PermissionName)).Distinct() ?? Enumerable.Empty<string>();

            var accessToken = JwtHelper.GenerateJwt(user, roles, permissions, _configuration);
            var refreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:expire"])),
                RefreshTokenExpiresAt = user.RefreshTokenExpiryTime,
                Roles = roles,
                Permissions = permissions,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return false;
            }

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiryDate = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == dto.Token);

            if (user == null || user.PasswordResetTokenExpiryDate <= DateTime.Now)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiryDate = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles!)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            var roles = user.UserRoles?.Select(ur => ur.Role.RoleName) ?? Enumerable.Empty<string>();
            var permissions = user.UserRoles?.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.PermissionName)).Distinct() ?? Enumerable.Empty<string>();

            var newAccessToken = JwtHelper.GenerateJwt(user, roles, permissions, _configuration);
            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:expire"])),
                RefreshTokenExpiresAt = user.RefreshTokenExpiryTime,
                Roles = roles,
                Permissions = permissions,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> VerifyEmailAsync(EmailVerificationDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == dto.Token && u.Email == dto.EmailVerification);

            if (user == null)
            {
                return false;
            }

            user.IsVerified = true;

            user.EmailVerificationToken = null;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}