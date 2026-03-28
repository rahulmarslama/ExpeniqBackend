using Expendiq.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Expendiq.Infrastructure.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {

        public string _connString = string.Empty;

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.HasDefaultSchema("idn");

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))

            {

                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            }

            builder.Entity<ApplicationUser>().HasIndex(a => a.UserName).IsUnique();

            builder.Entity<ApplicationRole>().HasIndex(a => a.Name).IsUnique();

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ApplicationRole> ApplicationRole { get; set; }
    }
}
