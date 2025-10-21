using DotLink.Domain.Entities;

namespace DotLink.Application.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}