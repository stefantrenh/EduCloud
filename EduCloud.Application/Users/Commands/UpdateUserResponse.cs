namespace EduCloud.Application.Users.Commands
{
    public record UpdateUserResponse(
        Guid UserId,
        string Message);
}