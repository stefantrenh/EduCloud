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
            return email.Contains("@");
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
