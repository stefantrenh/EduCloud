namespace EduCloud.Application.Users.Commands
{
    public record DeleteUserResponse(
        Guid UserId, string Message);
}