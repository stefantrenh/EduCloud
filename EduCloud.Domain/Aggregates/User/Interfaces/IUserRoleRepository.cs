
namespace EduCloud.Domain.Aggregates.User.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> GetUserRoleByIdAsync(Guid id);
        Task<List<UserRole>> GetRolesByUserIdAsync(Guid userId);
        Task AddUserRoleAsync(UserRole userRole);
        Task RemoveUserRoleAsync(Guid userRoleId);
    }

}
