using CreationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CreationApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email)
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .HasMaxLength(20);

                entity.Property(e => e.Surname)
                    .HasMaxLength(30);

                entity.Property(e => e.Age)
                    .HasMaxLength(5);
            });
        }
    }
}
