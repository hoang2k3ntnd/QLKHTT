using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; } = string.Empty;

        public string? Details { get; set; }
        public string? Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        public string IP { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
