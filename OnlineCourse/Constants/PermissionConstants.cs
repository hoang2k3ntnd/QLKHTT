// OnlineCourse/Constants/Permission.cs
namespace OnlineCourse.Constants
{
    public static class PermissionConstants
    {
        public static class User
        {
            public const string View = "User.View";
            public const string Manage = "User.Manage";
        }

        public static class Role
        {
            public const string View = "Role.View";
            public const string Create = "Role.Create";
            public const string Edit = "Role.Edit";
            public const string Delete = "Role.Delete";
        }

        public static class Course
        {
            public const string View = "Course.View";
            public const string Create = "Course.Create";
            public const string Edit = "Course.Edit";
            public const string Delete = "Course.Delete";
        }

        public static class Lesson
        {
            public const string View = "Lesson.View";
            public const string Create = "Lesson.Create";
            public const string Edit = "Lesson.Edit";
            public const string Delete = "Lesson.Delete";
        }

        public static class Payment
        {
            public const string View = "Payment.View";
            public const string Refund = "Payment.Refund";
        }

        public static class Log
        {
            public const string View = "Log.View";
        }
    }
}