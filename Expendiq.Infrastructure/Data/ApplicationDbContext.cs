using Expendiq.Domain.Entities;
using Expendiq.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace Expendiq.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("exp");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))

            {

                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            }

            base.OnModelCreating(modelBuilder);


            // Category configuration
            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).IsRequired().HasMaxLength(100);
                b.Property(c => c.Color).HasMaxLength(7);
                b.Property(c => c.Icon).HasMaxLength(50);

            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Description).IsRequired().HasMaxLength(500);
                b.Property(t => t.Amount).HasPrecision(18, 2);

                b.HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.RecurringTransaction)
                    .WithMany(r => r.Transactions)
                    .HasForeignKey(t => t.RecurringTransactionId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasIndex(t => t.Date);
            });

            // Budget configuration
            modelBuilder.Entity<Budget>(b =>
            {
                b.HasKey(b => b.Id);
                b.Property(b => b.Name).IsRequired().HasMaxLength(100);
                b.Property(b => b.LimitAmount).HasPrecision(18, 2);
                b.Property(b => b.Alert).HasDefaultValue(80);

                
                b.HasOne(b => b.Category)
                    .WithMany(c => c.Budgets)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            // RecurringTransaction configuration
            modelBuilder.Entity<RecurringTransaction>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.Description).IsRequired().HasMaxLength(500);
                b.Property(r => r.Amount).HasPrecision(18, 2);
                
                b.HasOne(r => r.Category)
                    .WithMany(c => c.RecurringTransactions)
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

    }
}
