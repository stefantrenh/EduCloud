using EduCloud.Domain.Aggregates.User;
using EduCloud.Domain.Aggregates.User.Interfaces;
using EduCloud.Domain.Aggregates.User.ValueObjects;
using EduCloud.Infrastructure.Constants;
using System.Data;

namespace EduCloud.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task AddAsync(User user)
        {
            const string sql = $"INSERT INTO {TableNames.Users} (Id, Fullname, Email, PasswordHash, CreatedDate) " +
                                "VALUES (@Id, @Fullname, @Email, @PasswordHash, @CreatedDate)";

            await ExecuteAsync(sql, new
            {
                user.Id,
                user.Fullname,
                Email = user.Email.Address,
                user.PasswordHash,
                user.CreatedDate
            });
        }

        public async Task DeleteAsync(Guid userId)
        {
            const string sql = $"DELETE FROM {TableNames.Users} WHERE Id = @Id";

            await ExecuteAsync(sql, new { Id = userId });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = $"SELECT Id, Fullname, Email, PasswordHash, CreatedDate FROM {TableNames.Users} WHERE Email = @Email";

            var result = await QueryFirstOrDefaultAsync<dynamic>(sql, new { Email = email });

            if (result == null)
            {
                return null;
            }
            var user = MapToUser(result);

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            const string sql = $"SELECT Id, Fullname, Email " +
                               $"FROM {TableNames.Users} WHERE Id = @Id";

            return await QueryFirstOrDefaultAsync<User>(sql, new { Id = userId });
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = $"UPDATE {TableNames.Users} " +
                               $"SET Fullname = @Fullname, Email = @Email " +
                               $"WHERE Id = @Id";

            await ExecuteAsync(sql, new { user.Fullname, user.Email, user.Id });
        }

        private User MapToUser(dynamic result)
        {
            var email = result.Email;
            var emailVO = Email.Create(email);
            var user = new User(
                result.Fullname,
                emailVO,
                result.PasswordHash,
                result.CreatedDate
            );

            return user;
        }
    }
}
