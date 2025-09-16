using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Helpers;
using OnlineCourse.Interfaces;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Log.View")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _logService.GetAllAsync();
            return Ok(ApiResponse.Success(
                action: "GetAllLogs",
                message: "Lấy danh sách log thành công",
                data: logs
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _logService.GetByIdAsync(id);
            if (log == null)
                return NotFound(ApiResponse.Fail("GetLogById", "Log không tồn tại", 404));

            return Ok(ApiResponse.Success(
                action: "GetLogById",
                message: "Lấy chi tiết log thành công",
                data: log
            ));
        }
    }
}
