
using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public record DeleteUserCommand(Guid UserId) : IRequest<DeleteUserResponse>;
    
}
