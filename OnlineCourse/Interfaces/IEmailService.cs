// OnlineCourse/Interfaces/IEmailService.cs
namespace OnlineCourse.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}