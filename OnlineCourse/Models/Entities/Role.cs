using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
