using System.Threading.Tasks;

namespace DotLink.Application.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string username, string resetLink);
    }
}