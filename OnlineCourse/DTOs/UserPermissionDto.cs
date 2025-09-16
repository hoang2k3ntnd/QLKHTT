namespace OnlineCourse.DTOs
{
    public class UserPermissionDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
