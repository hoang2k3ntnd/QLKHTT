using Microsoft.AspNetCore.Authorization;
using OnlineCourse.Helpers;

namespace OnlineCourse.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static void AddPermissionPolicies(this AuthorizationOptions options)
        {
            var permissions = PermissionHelper.GetAllPermissions();

            foreach (var permission in permissions)
            {
                options.AddPolicy(permission, policy =>
                    policy.RequireClaim("Permission", permission));
            }
        }
    }
}
