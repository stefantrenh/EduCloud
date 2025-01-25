using EduCloud.Domain.Aggregates.User.Interfaces;
using MediatR;

namespace EduCloud.Application.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();

            var userDtoList = users.Select(user => new UserDTO(user.Fullname, user.Email.Address)).ToList();

            var userReadonlyList = userDtoList.AsReadOnly();

            return new GetAllUsersQueryResponse(userReadonlyList, "Success");
        }
    }
}
