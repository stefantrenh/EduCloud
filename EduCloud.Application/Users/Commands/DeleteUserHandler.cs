
using EduCloud.Domain.Aggregates.User.Interfaces;
using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand,DeleteUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand command, CancellationToken cancellation)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);

            if (user == null) 
            {
                throw new ArgumentException("User does not exists.");
            }

            user.DeleteUser();

            await _userRepository.UpdateAsync(user);

            return new DeleteUserResponse(user.Id, $"User {user.Id} is now deleted and set to inactive.");
        }
    }
}
