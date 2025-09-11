using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        [DataType(DataType.Date)]
        public DateTime EnrolledAt { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
