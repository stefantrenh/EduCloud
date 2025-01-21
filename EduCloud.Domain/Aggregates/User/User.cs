using EduCloud.Domain.Aggregates.Common.Models;
using EduCloud.Domain.Aggregates.User.ValueObjects;


namespace EduCloud.Domain.Aggregates.User
{
    public class User : AggregateRoot
    {
        public string Fullname { get; private set; }
        public Email Email { get; private set; }
        public List<UserRole> Roles { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private User() { }

        public User(string fullName, Email email, string password, DateTime createdDate)
        {
            Fullname = fullName;
            Email = email;
            Roles = new List<UserRole>();
            PasswordHash = password;
            CreatedDate = createdDate;
        }

        public static User Create(string fullName, string email, string password) 
        { 
            var emailVO = Email.Create(email);
            return new User(fullName, emailVO, password, DateTime.UtcNow);
        }

        public static User Rehydrate(
            string fullName,
            Email email,
            List<UserRole> roles,
            string? passwordHash,
            DateTime createdDate)
        {
            var user = new User
            {
                Fullname = fullName,
                Email = email,
                Roles = roles ?? new List<UserRole>(),
                PasswordHash = passwordHash ?? string.Empty,
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

        public void ChangeEmail(string newEmail)
        {
            var newEmailVO = Email.Create(newEmail);
            Email = newEmailVO;
            UpdatedDateStamp();

        }
    }
}
