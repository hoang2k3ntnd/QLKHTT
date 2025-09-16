namespace OnlineCourse.DTOs
{
    public class LogOverallDto
    {
        public int StatusCode { get; set; }
        public string HttpMethod { get; set; } = string.Empty;
        public string RequestUrl { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string IP { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public long Duration { get; set; }
        public string? ApplicationName { get; set; }
        public string? BrowserInfo { get; set; }
        public string? Exception { get; set; }
        public string? CorrelationId { get; set; }
        public string? Comments { get; set; }
        public string? ExtraProperties { get; set; }
    }

    public class LogActionDto
    {
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; } // JSON nếu có
    }

    public class LogChangeDto
    {
        public string PropertyName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }

    public class LogDetailResponseDto
    {
        public LogOverallDto Overall { get; set; } = new LogOverallDto();
        public List<LogActionDto> Actions { get; set; } = new();
        public List<LogChangeDto> Changes { get; set; } = new();
    }
}
