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
    );
}
