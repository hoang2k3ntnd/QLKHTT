using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Models.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int EnrollmentId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Failed
        public string GatewayRef { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
    }
}
