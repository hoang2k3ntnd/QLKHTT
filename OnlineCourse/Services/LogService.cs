using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data;
using OnlineCourse.DTOs;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Services
{
    public class LogService : ILogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LogDetailResponseDto>> GetAllAsync()
        {
            var logs = await _context.Logs
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return logs.Select(l => new LogDetailResponseDto
            {
                Overall = new LogOverallDto
                {
                    StatusCode = l.StatusCode,
                    HttpMethod = l.HttpMethod,
                    RequestUrl = l.RequestUrl,
                    UserName = l.User?.UserName,
                    IP = l.IP,
                    CreatedAt = l.CreatedAt,
                    Duration = l.Duration,
                    ApplicationName = l.ApplicationName,
                    BrowserInfo = l.BrowserInfo,
                    Exception = l.Exception,
                    CorrelationId = l.CorrelationId,
                    Comments = l.Comments,
                    ExtraProperties = l.ExtraProperties
                },
                Actions = string.IsNullOrEmpty(l.Details)
                    ? new List<LogActionDto>()
                    : new List<LogActionDto> { new LogActionDto { Action = l.Action, Details = l.Details } },
                Changes = string.IsNullOrEmpty(l.Changes)
                    ? new List<LogChangeDto>()
                    : System.Text.Json.JsonSerializer.Deserialize<List<LogChangeDto>>(l.Changes) ?? new()
            });
        }

        public async Task<LogDetailResponseDto?> GetByIdAsync(int id)
        {
            var l = await _context.Logs.Include(u => u.User).FirstOrDefaultAsync(x => x.LogId == id);
            if (l == null) return null;

            return new LogDetailResponseDto
            {
                Overall = new LogOverallDto
                {
                    StatusCode = l.StatusCode,
                    HttpMethod = l.HttpMethod,
                    RequestUrl = l.RequestUrl,
                    UserName = l.User?.UserName,
                    IP = l.IP,
                    CreatedAt = l.CreatedAt,
                    Duration = l.Duration,
                    ApplicationName = l.ApplicationName,
                    BrowserInfo = l.BrowserInfo,
                    Exception = l.Exception,
                    CorrelationId = l.CorrelationId,
                    Comments = l.Comments,
                    ExtraProperties = l.ExtraProperties
                },
                Actions = string.IsNullOrEmpty(l.Details)
                    ? new List<LogActionDto>()
                    : new List<LogActionDto> { new LogActionDto { Action = l.Action, Details = l.Details } },
                Changes = string.IsNullOrEmpty(l.Changes)
                    ? new List<LogChangeDto>()
                    : System.Text.Json.JsonSerializer.Deserialize<List<LogChangeDto>>(l.Changes) ?? new()
            };
        }
        public async Task LogAction(string action, string? details, int? username = null, int statusCode = 200)
        {
            var log = new Log
            {
                UserName = username,
                Action = action,
                Details = details,
                StatusCode = statusCode,
                HttpMethod = "INTERNAL", // phân biệt log tự thêm
                RequestUrl = "SYSTEM",   // không có request trực tiếp
                IP = "127.0.0.1",
                ApplicationName = "OnlineCourse",
                CreatedAt = DateTime.Now,
                Duration = 0
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
