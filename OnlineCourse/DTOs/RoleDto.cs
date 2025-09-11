namespace OnlineCourse.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // RBAC
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
    }

    public class RoleCreateDto
    {
        public string RoleName { get; set; } = string.Empty;
        // optional: list of permission ids to assign on create

    }

    public class RoleUpdateDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

    }
}
