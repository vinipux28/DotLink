// DotLink.Infrastructure/Services/EmailServiceStub.cs

using DotLink.Application.Services;
using System;
using System.Threading.Tasks;

namespace DotLink.Infrastructure.Services
{
    public class EmailServiceConsole : IEmailService
    {
        public EmailServiceConsole(){}

        public Task SendPasswordResetEmailAsync(string toEmail, string username, string resetLink)
        {
            Console.WriteLine(
                "============================================================");
            Console.WriteLine($"--- EMAIL SERVICE STUB (Simulated Send) ---");
            Console.WriteLine($"TO: {toEmail} ({username})");
            Console.WriteLine($"SUBJECT: Password Reset Request");
            Console.WriteLine($"BODY: Hello {username}, you requested a password reset. Please use the link:");
            Console.WriteLine($"LINK: {resetLink}");
            Console.WriteLine("============================================================");

            return Task.CompletedTask;
        }
    }
}