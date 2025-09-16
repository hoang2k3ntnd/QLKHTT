namespace OnlineCourse.DTOs
{
    // Dùng khi tạo user
    using System.ComponentModel.DataAnnotations;

    public class UserCreateDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        public string? SurName { get; set; }

        [Phone]
        [StringLength(11, ErrorMessage = "Số điện thoại không được vượt quá 11 ký tự")]
        public string? NumberPhone { get; set; }
    }

    // Dùng khi update user

    public class UserUpdateDto
    {
        public int UserId { get; set; }

        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự")]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? SurName { get; set; }

        [Phone]
        [StringLength(11, ErrorMessage = "Số điện thoại không được vượt quá 11 ký tự")]
        public string? NumberPhone { get; set; }
    }
    // Dùng khi trả về response
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SurName { get; set; }
        public string? NumberPhone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }



        // RBAC
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
