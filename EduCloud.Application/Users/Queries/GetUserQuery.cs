
using MediatR;

namespace EduCloud.Application.Users.Queries
{
    public record GetUserQuery
    (Guid UserId) : IRequest<GetUserQueryResponse>;
}
