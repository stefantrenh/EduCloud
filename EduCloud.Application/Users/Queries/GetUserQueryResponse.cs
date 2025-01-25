namespace EduCloud.Application.Users.Queries
{
    public record class GetUserQueryResponse(
        UserDTO User,
        string Message);
}