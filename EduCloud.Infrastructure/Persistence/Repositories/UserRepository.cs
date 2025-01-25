using EduCloud.Domain.Aggregates.User;
using EduCloud.Domain.Aggregates.User.Interfaces;
using EduCloud.Domain.Aggregates.User.ValueObjects;
using EduCloud.Domain.Aggregates.User.Enums;
using EduCloud.Infrastructure.Constants;
using System.Data;

namespace EduCloud.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        //TODO: User Role

        public async Task AddAsync(User user)
        {
            const string sql = $"INSERT INTO {TableNames.Users} (Id, Fullname, Email, PasswordHash, UserStatus, CreatedDate) " +
                               "VALUES (@Id, @Fullname, @Email, @PasswordHash, @UserStatus, @CreatedDate)";

            var parameters = new
            {
                user.Id,
                user.Fullname,
                Email = user.Email.Address,
                user.PasswordHash,
                UserStatus = (int)user.UserStatus,
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
            const string sql = $"SELECT Id, Fullname, Email, UserStatus, CreatedDate " +
                               $"FROM {TableNames.Users} WHERE Email = @Email";

            var parameters = new { Email = email };
            var result = await QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            if (result == null)
                return null;

            var emailVO = Email.Create(result.Email);

            return MapRehydrateUserData(result);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            const string sql = @"SELECT Id, Fullname, Email, UserStatus, CreatedDate 
                                 FROM Users 
                                 WHERE Id = @Id";

            var parameters = new { Id = userId };
            var result = await QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            if (result == null)
                return null;

            return MapRehydrateUserData(result);
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = $"UPDATE {TableNames.Users} " + 
                               $"SET Fullname = @Fullname, Email = @Email, UserStatus = @UserStatus " +
                               $"WHERE Id = @Id";

            var parameters = new
            {
                Fullname = user.Fullname,
                Email = user.Email?.Address,
                UserStatus = (int)user.UserStatus,
                Id = user.Id
            };

            await ExecuteAsync(sql, parameters);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            const string sql = "SELECT Id, Fullname, Email, UserStatus, CreatedDate FROM Users";
            var userDtos = await QueryAsync<UserDto>(sql);

            var userList = userDtos.Select(dto =>
            {        
                return MapRehydrateUserData(dto);
            }).ToList();

            return userList ?? Enumerable.Empty<User>();
        }

        private User MapRehydrateUserData(UserDto dto)
        {

            var emailVO = Email.Create(dto.Email);
            var userStatus = (UserStatus)dto.UsersStatus;

            return User.Rehydrate(
                dto.Id,
                dto.Fullname,
                emailVO,
                new List<UserRole>(),
                "",
                userStatus,
                dto.CreatedDate
            );
        }

        private class UserDto
        {
            public Guid Id { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public int UsersStatus { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
