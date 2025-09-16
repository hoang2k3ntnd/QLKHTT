using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineCourse.Data;
using OnlineCourse.Data.Seeders;
using OnlineCourse.Extensions;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;
using OnlineCourse.Middlewares;
using OnlineCourse.Repositories;
using OnlineCourse.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// Đăng ký dịch vụ
// ==========================================================

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository & Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpContextAccessor();

// JWT Authentication
var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Policy-based Authorization (RBAC động)
builder.Services.AddAuthorization(options =>
{
    options.AddPermissionPolicies();
});

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Course API", Version = "v1" });

    // Swagger JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Khai báo schema cho ApiResponse để Swagger không còn báo "Undocumented"
    c.MapType<ApiResponse>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["statusCode"] = new OpenApiSchema { Type = "integer", Format = "int32" },
            ["action"] = new OpenApiSchema { Type = "string" },
            ["message"] = new OpenApiSchema { Type = "string" },
            ["error"] = new OpenApiSchema { Type = "string" },
            ["data"] = new OpenApiSchema { Type = "object", AdditionalPropertiesAllowed = true }
        }
    });
});

var app = builder.Build();

// ==========================================================
// Pipeline
// ==========================================================
app.UseRequestLogging();

// Auto migrate + seed dữ liệu mặc định (roles, permissions, admin user)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    PermissionSeeder.Seed(context);
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();
