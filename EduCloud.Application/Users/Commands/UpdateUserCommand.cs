using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public record UpdateUserCommand(
        Guid UserId,
        //List<UserRole> UserRoles,
        string FirstName,
        string LastName,
        string Email) : IRequest<UpdateUserResponse>;
}
