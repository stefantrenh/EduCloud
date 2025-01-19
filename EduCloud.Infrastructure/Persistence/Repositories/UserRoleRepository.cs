using EduCloud.Domain.Aggregates.User.Interfaces;
using EduCloud.Domain.Aggregates.User;
using EduCloud.Infrastructure.Constants;
using System.Data;

namespace EduCloud.Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public UserRoleRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public async Task<UserRole> GetUserRoleByIdAsync(Guid id)
        {
            const string sql = $"SELECT * FROM {TableNames.UserRole} WHERE Id = @Id";

            return await QueryFirstOrDefaultAsync<UserRole>(sql, new { Id = id });
        }

        public async Task<List<UserRole>> GetRolesByUserIdAsync(Guid userId)
        {
            const string sql = $"SELECT * FROM {TableNames.UserRole} WHERE UserId = @UserId";

            var roles = await QueryAsync<UserRole>(sql, new { UserId = userId });

            return roles.ToList();
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            const string sql = $"INSERT INTO {TableNames.UserRole} (Id, UserId, Role) VALUES (@Id, @UserId, @Role)";

            await ExecuteAsync(sql, new
            {
                userRole.Id,
                userRole.UserId,
                userRole.Name
            });
        }

        public async Task RemoveUserRoleAsync(Guid userRoleId)
        {
            const string sql = $"DELETE FROM {TableNames.UserRole} WHERE Id = @Id";
            await ExecuteAsync(sql, new { Id = userRoleId });
        }
    }
}
