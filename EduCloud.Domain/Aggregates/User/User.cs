using EduCloud.Domain.Aggregates.Common.Models;
using EduCloud.Domain.Aggregates.User.Enums;
using EduCloud.Domain.Aggregates.User.ValueObjects;


namespace EduCloud.Domain.Aggregates.User
{
    public class User : AggregateRoot
    {
        public string Fullname { get; private set; }
        public Email Email { get; private set; }
        public List<UserRole> Roles { get; private set; }
        public string PasswordHash { get; private set; }
        public UserStatus UserStatus { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private User() { }

        public User(string fullName, Email email, string password, UserStatus status, DateTime createdDate)
        {
            Fullname = fullName;
            Email = email;
            Roles = new List<UserRole>();
            PasswordHash = password;
            UserStatus = status;
            CreatedDate = createdDate;
        }

        public static User Create(string fullName, string email, string password) 
        { 
            var emailVO = Email.Create(email);
            return new User(fullName, emailVO, password, UserStatus.Active ,DateTime.UtcNow);
        }

        public static User Rehydrate(
            Guid userId,
            string fullName,
            Email email,
            List<UserRole> roles,
            string? passwordHash,
            UserStatus userStatus,
            DateTime createdDate)
        {
            var user = new User
            {
                Id = userId,
                Fullname = fullName,
                Email = email,
                Roles = roles ?? new List<UserRole>(),
                PasswordHash = passwordHash ?? string.Empty,
                UserStatus = userStatus,
                CreatedDate = createdDate
            };

            return user;
        }

        public void AddRole(UserRole role)
        {
            if (Roles.Contains(role))
                throw new InvalidOperationException("Role already assigned.");

            Roles.Add(role);
            UpdatedDateStamp();
        }

        public void ChangeFullname(string newFullname)
        {
            if (string.IsNullOrWhiteSpace(newFullname))
                throw new ArgumentException("Fullname cannot be empty or whitespace.", nameof(newFullname));

            if (UserStatus == UserStatus.Inactive)
                throw new InvalidOperationException("Cannot change a user who is not active.");

            Fullname = newFullname;
            UpdatedDateStamp();
        }

        public void ChangeEmail(string newEmail)
        {
            if (UserStatus == UserStatus.Inactive) 
                throw new InvalidOperationException("Cannot change a user who is not active.");

            var newEmailVO = Email.Create(newEmail);
            Email = newEmailVO;
            UpdatedDateStamp();

        }

        public void SetStatus(UserStatus status)
        { 
            UserStatus = status;
            UpdatedDateStamp();
        }

        public void DeleteUser()
        {
            if (UserStatus == UserStatus.Inactive)
                throw new InvalidOperationException("Cannot delete a user who is not active.");


            SetStatus(UserStatus.Inactive);
            UpdatedDateStamp();
        }

        private bool IsActive()
        {
            return UserStatus == UserStatus.Active;
        }
    }
}
