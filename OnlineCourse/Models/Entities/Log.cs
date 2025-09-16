using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Log
    {

        public int LogId { get; set; }

        public int? UserName { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        public string? Details { get; set; }   // JSON cho action details
        public string? Changes { get; set; }   // JSON cho changes

        [Required]
        public int StatusCode { get; set; }

        [Required]
        public string HttpMethod { get; set; } = string.Empty;

        [Required]
        public string RequestUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string IP { get; set; } = string.Empty;

        public string? ApplicationName { get; set; }
        public string? BrowserInfo { get; set; }
        public string? Exception { get; set; }
        public string? CorrelationId { get; set; }
        public string? Comments { get; set; }
        public string? ExtraProperties { get; set; }

        public DateTime CreatedAt { get; set; }
        public long Duration { get; set; }


        public User? User { get; set; }
    }
}
