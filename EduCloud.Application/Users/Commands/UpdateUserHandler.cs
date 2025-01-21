using EduCloud.Domain.Aggregates.User.Interfaces;
using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    { 
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.ChangeEmail(command.Email);
            user.ChangeFullname($"{command.FirstName} {command.LastName}");

            Console.WriteLine($"Updated user: {user.Fullname}, {user.Email?.Address}");

            await _userRepository.UpdateAsync(user);

            return new UpdateUserResponse(user.Id, $"Successfully updated user: {user.Id}");
        }

    }
}
