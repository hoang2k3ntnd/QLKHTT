using OnlineCourse.DTOs;

namespace OnlineCourse.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<LogDetailResponseDto>> GetAllAsync();
        Task<LogDetailResponseDto?> GetByIdAsync(int id);

        Task LogAction(string action, string? details, int? userId = null, int statusCode = 200);

    }
}
