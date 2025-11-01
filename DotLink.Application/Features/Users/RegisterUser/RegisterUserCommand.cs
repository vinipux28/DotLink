using MediatR;
using System;

namespace DotLink.Application.Commands.UserCommands
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterUserCommand(string email, string username, string password)
        {
            Email = email;
            Username = username;
            Password = password;
        }
    }
}