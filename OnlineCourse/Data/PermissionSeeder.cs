// OnlineCourse/Data/Seeders/PermissionSeeder.cs
using OnlineCourse.Constants;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Data.Seeders
{
    public static class PermissionSeeder
    {
        public static List<Permission> GetAllPermissions()
        {
            return new List<Permission>
            {
                new Permission { PermissionName = PermissionConstants.User.View },
                new Permission { PermissionName = PermissionConstants.User.Manage },
                new Permission { PermissionName = PermissionConstants.Role.View },
                new Permission { PermissionName = PermissionConstants.Role.Create },
                new Permission { PermissionName = PermissionConstants.Role.Edit },
                new Permission { PermissionName = PermissionConstants.Role.Delete },
                new Permission { PermissionName = PermissionConstants.Course.View },
                new Permission { PermissionName = PermissionConstants.Course.Create },
                new Permission { PermissionName = PermissionConstants.Course.Edit },
                new Permission { PermissionName = PermissionConstants.Course.Delete },
                new Permission { PermissionName = PermissionConstants.Lesson.View },
                new Permission { PermissionName = PermissionConstants.Lesson.Create },
                new Permission { PermissionName = PermissionConstants.Lesson.Edit },
                new Permission { PermissionName = PermissionConstants.Lesson.Delete },
                new Permission { PermissionName = PermissionConstants.Payment.View },
                new Permission { PermissionName = PermissionConstants.Payment.Refund },
                new Permission { PermissionName = PermissionConstants.Log.View }
            };
        }

        public static void Seed(AppDbContext context)
        {
            // Seed Permissions
            if (!context.Permissions.Any())
            {
                var permissions = GetAllPermissions();
                context.Permissions.AddRange(permissions);
                context.SaveChanges();
            }

            // Seed Roles
            if (!context.Roles.Any())
            {
                var adminRole = new Role { RoleName = "Admin" };
                var teacherRole = new Role { RoleName = "Teacher" };
                var studentRole = new Role { RoleName = "Student" };

                context.Roles.AddRange(adminRole, teacherRole, studentRole);
                context.SaveChanges(); // => Sau khi save, Id sẽ tự được gán

                // Gán tất cả quyền cho Admin
                var allPermissions = context.Permissions.ToList();
                foreach (var permission in allPermissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.RoleId,   // đã có Id sau SaveChanges
                        PermissionId = permission.PermissionId
                    });
                }

                // Teacher: chỉ Course + Lesson
                var teacherPermissions = context.Permissions
                    .Where(p => p.PermissionName.StartsWith("Course.") || p.PermissionName.StartsWith("Lesson."))
                    .ToList();
                foreach (var permission in teacherPermissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = teacherRole.RoleId,
                        PermissionId = permission.PermissionId
                    });
                }

                // Student: chỉ có quyền View
                var studentPermissions = context.Permissions
                    .Where(p => p.PermissionName.EndsWith(".View"))
                    .ToList();
                foreach (var permission in studentPermissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = studentRole.RoleId,
                        PermissionId = permission.PermissionId
                    });
                }

                context.SaveChanges();
            }

            // Seed Admin User mặc định
            if (!context.Users.Any(u => u.Email == "admin@example.com"))
            {
                var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                if (adminRole != null)
                {
                    var adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminpass"),
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    context.Users.Add(adminUser);
                    context.SaveChanges(); // => Id user cũng tự sinh

                    context.UserRoles.Add(new UserRole
                    {
                        UserId = adminUser.UserId,
                        RoleId = adminRole.RoleId
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
