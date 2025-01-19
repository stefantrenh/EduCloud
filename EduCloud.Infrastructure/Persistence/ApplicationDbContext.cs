using EduCloud.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace EduCloud.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Fullname).IsRequired().HasMaxLength(100);

                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Address)
                         .HasColumnName("Email")
                         .IsRequired()
                         .HasMaxLength(200);
                });

                entity.HasMany(u => u.Roles)
                    .WithOne()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });
        }
    }
}
