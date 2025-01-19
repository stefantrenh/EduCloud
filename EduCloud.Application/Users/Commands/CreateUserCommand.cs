using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public record CreateUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role
        ) : IRequest<CreateUserResponse>;
}
