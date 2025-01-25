using EduCloud.Domain.Aggregates.User;
using EduCloud.Domain.Aggregates.User.Enums;
using EduCloud.Domain.Aggregates.User.ValueObjects;
using FluentAssertions;
using Xunit;


namespace EduCloud.Test.UnitTest.UserTests
{
    public class UserDomainTest
    {
        protected string fullName;
        protected Email email;
        protected string password;
        protected DateTime createdDate;
        protected UserStatus status;
        protected List<UserRole> roles;
        public UserDomainTest()
        {
            fullName = "Jörgen Rönning";
            email = Email.Create("Jörgenrönning@testmail.com");
            password = "password"; //no passwordvalidation yet 
            createdDate = DateTime.UtcNow;
            roles = new List<UserRole>();
            status = UserStatus.Active;
        }
    }

    public class CreateUserWithValidDataTest : UserDomainTest
    {
        [Fact]
        public void ShouldReturnExpectedMockData()
        {
            var result = User.Create(fullName,email.Address,password);

            result.Fullname.Should().Be(fullName);
            result.Email.Should().Be(email);
            result.PasswordHash.Should().Be(password);
            result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }

    public class CreateUserWithInvalidEmailTest : UserDomainTest
    {
        [Fact]
        public void ShouldThrowExceptionWithMessage()
        {
            Action act = () => User.Create(fullName, "invaildEmail", password);

            act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
        }
    }

    public class ChangeFullnameTest: UserDomainTest 
    {
        [Fact]
        public void ShouldBeEquivalent()
        {
            var user = User.Create(fullName, email.Address, password);
            var newFullName = "Andreas Öhman";

            user.ChangeFullname(newFullName);

            user.Fullname.Should().BeEquivalentTo(newFullName);
        }
    }

    public class InvalidFullNameTest : UserDomainTest
    {
        [Fact]
        public void ShouldThrowExceptionWithMessage() 
        {
            var user = User.Create(fullName, email.Address, password);

            Action act = () => user.ChangeFullname("");

            act.Should().Throw<ArgumentException>()
               .Where(e => e.Message.Contains("Fullname cannot be empty or whitespace"));
        }
    }

    public class SoftDeleteUserTest : UserDomainTest
    {
        [Fact]
        public void ShouldBeInActive()
        {
            var result = User.Create(fullName, email.Address, password);

            result.DeleteUser();

            result.UserStatus.Should().Be(UserStatus.Inactive);
        }
    }

    public class RehydrateUserShouldReturnValidData : UserDomainTest
    {
        [Fact]
        public void ShouldContainData()
        {
            var user = User.Rehydrate(
                Guid.NewGuid(),
                fullName,
                email,
                roles,
                "",
                status,
                createdDate);

            user.Fullname.Should().Be(fullName);
            user.Email.Should().Be(email);
            user.Roles.Should().BeEquivalentTo(roles);
            user.PasswordHash.Should().Be("");
            user.UserStatus.Should().Be(UserStatus.Active);
            user.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            
        }
    }
}
