using Microsoft.AspNetCore.Authorization;
using OnlineCourse.Constants;
using System.Reflection;

namespace OnlineCourse.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        /// <summary>
        /// Quét PermissionConstants và đăng ký Policy cho từng quyền
        /// </summary>
        public static void AddPermissionPolicies(this AuthorizationOptions options)
        {
            var permissions = typeof(PermissionConstants)
                .GetNestedTypes()
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetValue(null)?.ToString())
                .Where(v => !string.IsNullOrEmpty(v));

            foreach (var permission in permissions!)
            {
                options.AddPolicy(permission!, policy =>
                    policy.RequireClaim("Permission", permission!));
            }
        }
    }
}
