namespace EduCloud.Domain.Aggregates.User.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
    }
}
