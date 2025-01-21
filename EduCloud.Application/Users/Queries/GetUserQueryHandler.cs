using EduCloud.Application.DTO.UserDTO;
using EduCloud.Domain.Aggregates.User.Interfaces;
using MediatR;

namespace EduCloud.Application.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        { 
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var userDTO = new UserDTO(user.Fullname , user.Email.Address);

            var userResponse = new GetUserQueryResponse(userDTO, "Success");
            return userResponse;
        }

    }
}
