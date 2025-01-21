using EduCloud.Application.DTO.UserDTO;
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

            var parameters = new
            {
                user.Id,
                user.Fullname,
                Email = user.Email.Address,
                user.PasswordHash,
                user.CreatedDate
            };

            await ExecuteAsync(sql, parameters);
        }

        public async Task DeleteAsync(Guid userId)
        {
            const string sql = $"DELETE FROM {TableNames.Users} WHERE Id = @Id";

            var parameters = new { Id = userId };

            await ExecuteAsync(sql, parameters);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = $"SELECT Id, Fullname, Email, PasswordHash, CreatedDate " +
                               $"FROM {TableNames.Users} WHERE Email = @Email";

            var parameters = new { Email = email };
            var result = await QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            if (result == null)
                return null;

            var emailVO = Email.Create(result.Email);
            return new User(
                result.Fullname,
                emailVO,
                result.PasswordHash,
                result.CreatedDate
            );
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            const string sql = @"SELECT Id, Fullname, Email, PasswordHash, CreatedDate 
                                 FROM Users 
                                 WHERE Id = @Id";

            var parameters = new { Id = userId };
            var result = await QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            if (result == null)
                return null;

            var emailVO = Email.Create(result.Email);
            var roles = new List<UserRole>(); 

            return User.Rehydrate(
                result.Id,
                result.Fullname,
                emailVO,
                roles,
                result.PasswordHash,
                result.CreatedDate
            );
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = $"UPDATE {TableNames.Users} " +
                               $"SET Fullname = @Fullname, Email = @Email " +
                               $"WHERE Id = @Id";

            var parameters = new
            {
                Fullname = user.Fullname,
                Email = user.Email?.Address,
                Id = user.Id
            };

            await ExecuteAsync(sql, parameters);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            const string sql = "SELECT Fullname, Email FROM Users";
            var userDtos = await QueryAsync<UserDTO>(sql);

            var userList = userDtos.Select(dto =>
            {
                return User.CreateFromFullnameAndEmail(dto.Fullname, dto.Email);
            }).ToList();

            return userList ?? Enumerable.Empty<User>();
        }

        private class UserDto
        {
            public Guid Id { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
