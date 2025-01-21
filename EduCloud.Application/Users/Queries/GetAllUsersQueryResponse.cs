using EduCloud.Application.DTO.UserDTO;

namespace EduCloud.Application.Users.Queries
{
    public class GetAllUsersQueryResponse
    {
        public IReadOnlyList<UserDTO> Users { get; }
        public string Message { get; }

        public GetAllUsersQueryResponse(IReadOnlyList<UserDTO> users, string message)
        {
            Users = users;
            Message = message;
        }
    }

    public record class UserDTO(string Fullname, string Email);
}