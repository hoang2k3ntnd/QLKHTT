using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự")]
        public string? UserName { get; set; }
        public string? SurName { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        [Phone]
        [StringLength(11, ErrorMessage = "số điện thoại không được vượt quá 11 ký tự")]
        public string? NumberPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiryDate { get; set; }

        public bool IsEmailVerified { get; set; }
        public string? EmailVerificationToken { get; set; }


        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<Log>? Logs { get; set; }

    }
}
