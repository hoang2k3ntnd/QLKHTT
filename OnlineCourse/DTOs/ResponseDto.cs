namespace OnlineCourse.DTOs
{
    /// <summary>
    /// Dùng để trả kết quả chung từ API (OK / lỗi)
    /// </summary>
    public class ResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static ResponseDto Ok(string msg, object? data = null) =>
            new ResponseDto { Success = true, Message = msg, Data = data };

        public static ResponseDto Fail(string msg, object? data = null) =>
            new ResponseDto { Success = false, Message = msg, Data = data };
    }

    /// <summary>
    /// Dùng cho phân trang (trả về danh sách + tổng số item)
    /// </summary>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
    }
}
