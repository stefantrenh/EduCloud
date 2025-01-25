using MediatR;

namespace EduCloud.Application.Users.Queries
{
    public record GetAllUsersQuery : IRequest<GetAllUsersQueryResponse>;
}
