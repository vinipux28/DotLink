using MediatR;
using System;

namespace DotLink.Application.Features.Users.LoginUser
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Term { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string term, string password)
        {
            Term = term;
            Password = password;
        }
    }
}