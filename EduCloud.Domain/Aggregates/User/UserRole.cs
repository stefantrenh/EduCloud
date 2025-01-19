using EduCloud.Domain.Aggregates.Common.Models;
using EduCloud.Domain.Aggregates.User.Enums;

namespace EduCloud.Domain.Aggregates.User
{
    public class UserRole : Entity
    {
        public RoleEnum Name { get; private set; }

        public Guid UserId { get; set; }

        public UserRole(RoleEnum name, Guid userId)
        {
            if (!Enum.IsDefined(typeof(RoleEnum), name))
            {
                throw new ArgumentException($"Invalid role: {name}");
            }

            Name = name;
            UserId = userId;
        }

        public static UserRole Create(RoleEnum name, Guid userId) 
        { 
            return new UserRole(name, userId);
        }
    }
}
