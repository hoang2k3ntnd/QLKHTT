using OnlineCourse.Constants;
using System.Reflection;

namespace OnlineCourse.Helpers
{
    public static class PermissionHelper
    {
        /// <summary>
        /// Quét toàn bộ quyền từ PermissionConstants (const string).
        /// </summary>
        public static IEnumerable<string> GetAllPermissions()
        {
            return typeof(PermissionConstants)
                .GetNestedTypes(BindingFlags.Public)
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!)
                .ToList();
        }
    }
}
