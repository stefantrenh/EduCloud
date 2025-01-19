namespace EduCloud.Domain.Aggregates.Common.Models
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public DateTime? LastUpdated { get; protected set; }

        protected AggregateRoot()
        {
            Id = Guid.NewGuid();
            LastUpdated = DateTime.UtcNow;
        }

        public void UpdatedDateStamp()
        {
            LastUpdated = DateTime.UtcNow;
        }
    }
}
