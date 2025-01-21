using EduCloud.Domain.Aggregates.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCloud.Application.Users.Commands
{
    public record UpdateUserCommand(
        Guid UserId,
        List<UserRole> UserRoles,
        string FirstName,
        string LastName) : IRequest<UpdateUserResponse>;
}
