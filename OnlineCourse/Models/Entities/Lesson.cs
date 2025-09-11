using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Content { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public Course Course { get; set; } = null!;
    }
}
