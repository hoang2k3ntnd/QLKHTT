namespace OnlineCourse.DTOs
{
    // Dùng khi tạo user
    public class UserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SurName { get; set; }
        public string? NumberPhone { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    // Dùng khi update user
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? SurName { get; set; }
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
