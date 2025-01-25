using System.Text.RegularExpressions;

namespace EduCloud.Domain.Aggregates.User.ValueObjects
{
    public class Email
    {
        public string Address { get; private set; }

        private Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address cannot be empty");

            if (!IsValidEmail(address))
                throw new ArgumentException("Invalid email format.");

            Address = address;
        }

        private bool IsValidEmail(string email)
        {
            //validates a-ö, 2 char before and 3 after 
            string emailRegex = @"^[\w\.\-åäöüÅÄÖÜ]{2,}@[a-zA-Z0-9\-åäöüÅÄÖÜ]{3,}\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        public static Email Create(string address) 
        { 
            return new Email(address);
        }

        public override bool Equals(object obj)
        {
            return obj is Email email && email.Address == Address;
        }

        public override int GetHashCode() 
        { 
            return Address.GetHashCode();
        }
    }
}
