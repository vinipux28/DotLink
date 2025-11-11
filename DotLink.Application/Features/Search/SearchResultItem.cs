using System;

namespace DotLink.Application.Features.Search
{
    public enum EntityType
    {
        User,
        Post
    }

    public record SearchResultItem(
        Guid Id,
        EntityType Type,
        string TitleOrUsername,
        string PreviewContent,
        double RelevanceScore,
        string? ImageKey
    )
    {
        public SearchResultItem(DotLink.Domain.Entities.User user)
            : this(
                  user.Id,
                  EntityType.User,
                  user.Username,
                  string.Empty,
                  1.0,
                  user.ProfilePictureKey
              )
        { }
        public SearchResultItem(DotLink.Domain.Entities.Post post)
            : this(
                  post.Id,
                  EntityType.Post,
                  post.Title,
                  post.Content.Length <= 100 ? post.Content : post.Content.Substring(0, 100) + "...",
                  1.0,
                  null
              )
        { }
    };
}
