using Graph.CrossCutting;
using Graph.Infrastructure.Database.Command;
using Graph.Infrastructure.Database.Command.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Graph.Infrastructure.Database.Command
{
    public class GraphContext : DbContext
    {
        public GraphContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var statuses = Enum.GetValues(typeof(TaskStatusEnum)).OfType<TaskStatusEnum>().Select(i => new Status() { Id = (int)i, Description = i.ToString() });

            modelBuilder.Entity<Status>().HasData(statuses);

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(i => i.Email).IsUnique();
                e.Property(i => i.Email).HasMaxLength(100).IsRequired();
                e.Property(i => i.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Project>(e =>
            {
                e.HasMany(i => i.Tasks).WithOne(i => i.Project);
            });

            modelBuilder.Entity<UserProject>(e =>
            {
                e.Ignore(i => i.Id);
                e.HasKey(i => new { i.Projectid, i.UserId });
            });
        }
    }
}
