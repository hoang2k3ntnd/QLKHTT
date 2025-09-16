// OnlineCourse/Data/Seeders/PermissionSeeder.cs
using OnlineCourse.Helpers;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Data.Seeders
{
    public static class PermissionSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // 1. Seed Permissions
            if (!context.Permissions.Any())
            {
                var permissions = PermissionHelper.GetAllPermissions()
                    .Select(p => new Permission { PermissionName = p })
                    .ToList();

                context.Permissions.AddRange(permissions);
                context.SaveChanges();
            }

            // 2. Seed Roles
            if (!context.Roles.Any())
            {
                var adminRole = new Role { RoleName = "Admin" };
                var teacherRole = new Role { RoleName = "Teacher" };
                var studentRole = new Role { RoleName = "Student" };

                context.Roles.AddRange(adminRole, teacherRole, studentRole);
                context.SaveChanges(); // => có RoleId

                var allPermissions = context.Permissions.ToList();

                // Admin → tất cả quyền
                foreach (var permission in allPermissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.RoleId,
                        PermissionId = permission.PermissionId
                    });
                }

                // Teacher → quyền liên quan Course & Lesson
                var teacherPermissions = allPermissions
                    .Where(p => p.PermissionName.StartsWith("Course.") ||
                                p.PermissionName.StartsWith("Lesson."))
                    .ToList();

                foreach (var permission in teacherPermissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = teacherRole.RoleId,
                        PermissionId = permission.PermissionId
                    });
                }

                // Student → chỉ các quyền View
                var studentPermissions = allPermissions
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

            // 3. Seed Admin User mặc định
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
                    context.SaveChanges(); // => UserId có giá trị

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
