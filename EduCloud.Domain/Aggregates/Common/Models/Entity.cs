namespace EduCloud.Domain.Aggregates.Common.Models
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
