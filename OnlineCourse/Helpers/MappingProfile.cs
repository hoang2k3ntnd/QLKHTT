namespace OnlineCourse.Helpers
{
    using AutoMapper;
    using OnlineCourse.DTOs;
    using OnlineCourse.Models.Entities;
    using System.Linq;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // =========================
            // 🔹 User
            // =========================
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName)));

            CreateMap<UserDto, User>();

            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            // lưu ý: PasswordHash cần hash trong Service, không map trực tiếp như vậy khi production

            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // =========================
            // 🔹 Role
            // =========================
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src =>
                    src.RolePermissions.Select(rp => rp.Permission.PermissionName)));

            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();


            // =========================
            // 🔹 Permission
            // =========================
            //CreateMap<Permission, PermissionDto>()
            //    .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId))
            //    .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.PermissionName));

            //CreateMap<PermissionCreateDto, Permission>();
            //CreateMap<PermissionUpdateDto, Permission>();


            // =========================
            // 🔹 Course
            // =========================
            //CreateMap<Course, CourseDto>()
            //    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            //CreateMap<CreateCourseDto, Course>();
            //CreateMap<UpdateCourseDto, Course>();


            // =========================
            // 🔹 Lesson
            // =========================
            //CreateMap<Lesson, LessonDto>()
            //    .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));

            //CreateMap<CreateLessonDto, Lesson>();
            //CreateMap<UpdateLessonDto, Lesson>();


            // =========================
            // 🔹 Enrollment
            // =========================
            //CreateMap<Enrollment, EnrollmentDto>()
            //    .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.EnrollmentId))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

            //CreateMap<CreateEnrollmentDto, Enrollment>();


            // =========================
            // 🔹 Payment
            // =========================
            //CreateMap<Payment, PaymentDto>()
            //    .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            //CreateMap<CreatePaymentDto, Payment>();


            // =========================
            // 🔹 Log
            // =========================
            //CreateMap<Log, LogDto>()
            //    .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogId))
            //    .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));


            // =========================
            // 🔹 Auth
            // =========================
            //CreateMap<LoginDto, User>();
            //CreateMap<User, AuthResponseDto>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName)));
        }
    }
}
