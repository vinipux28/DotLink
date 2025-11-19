using DotLink.Infrastructure.Services;
using DotLink.Domain.Entities;
using Xunit;
using System;

namespace DotLink.Tests
{
    public class DTOMapperServiceTests
    {
        [Fact]
        public void MapToUserDTO_Maps_Properties_Correctly()
        {
            var mapper = new DTOMapperService();

            var user = new User(Guid.NewGuid(), "johndoe", "john@example.com", "hash");
            user.UpdateName("John", "Doe");
            user.UpdateProfilePictureKey("key123");

            var dto = mapper.MapToUserDTO(user);

            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Username, dto.Username);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.ProfilePictureKey, dto.ProfilePictureKey);
            Assert.Equal(user.Followers.Count, dto.FollowersCount);
        }
    }
}
