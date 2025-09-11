namespace OnlineCourse.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? Error { get; set; }
        public object? Data { get; set; }

        /// <summary>
        /// Trả về response thành công
        /// </summary>
        public static ApiResponse Success(string action, string message, object? data = null, int statusCode = 200)
            => new()
            {
                StatusCode = statusCode,
                Action = action,
                Message = message,
                Data = data
            };

        /// <summary>
        /// Trả về response thất bại
        /// </summary>
        public static ApiResponse Fail(string action, string error, int statusCode = 400)
            => new()
            {
                StatusCode = statusCode,
                Action = action,
                Error = error
            };
    }
}

