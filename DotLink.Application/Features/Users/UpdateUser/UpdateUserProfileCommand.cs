using MediatR;

public class UpdateUserProfileCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string? NewUsername { get; set; }
    public string? NewBio { get; set; }

    public Stream? ProfilePictureStream { get; set; } // File contents
    public string? ProfilePictureFileName { get; set; } // File name
    public string? ProfilePictureContentType { get; set; } // Type (image/jpeg)

    public bool RemoveProfilePicture { get; set; }
}