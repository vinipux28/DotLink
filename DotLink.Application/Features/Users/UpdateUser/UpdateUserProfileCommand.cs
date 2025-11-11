using MediatR;

public class UpdateUserProfileCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string newFirstName { get; set; } = string.Empty;
    public string newLastName { get; set; } = string.Empty;
    public string? NewUsername { get; set; }
    public string? NewBio { get; set; }

}