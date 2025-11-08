using MediatR;

public class UpdateUserProfileCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string? NewUsername { get; set; }
    public string? NewBio { get; set; }

}