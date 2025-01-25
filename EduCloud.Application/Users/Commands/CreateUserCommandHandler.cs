using EduCloud.Application.Common.Exceptions;
using EduCloud.Domain.Aggregates.User;
using EduCloud.Domain.Aggregates.User.Enums;
using EduCloud.Domain.Aggregates.User.Interfaces;
using MediatR;

namespace EduCloud.Application.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, IUserRoleRepository roleRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        { 
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                throw new ConflictException("User email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = User.Create($"{request.FirstName} {request.LastName}", request.Email,hashedPassword);

            await _userRepository.AddAsync(user);

            return new CreateUserResponse(user.Id, "User created");
        }
    }
}
