using Expendiq.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expendiq.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser configuration
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("AspNetUsers");
            });

            // Category configuration
            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).IsRequired().HasMaxLength(100);
                b.Property(c => c.Color).HasMaxLength(7);
                b.Property(c => c.Icon).HasMaxLength(50);

                b.HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Description).IsRequired().HasMaxLength(500);
                b.Property(t => t.Amount).HasPrecision(18, 2);

                b.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.RecurringTransaction)
                    .WithMany(r => r.Transactions)
                    .HasForeignKey(t => t.RecurringTransactionId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasIndex(t => t.UserId);
                b.HasIndex(t => t.Date);
            });

            // Budget configuration
            modelBuilder.Entity<Budget>(b =>
            {
                b.HasKey(b => b.Id);
                b.Property(b => b.Name).IsRequired().HasMaxLength(100);
                b.Property(b => b.LimitAmount).HasPrecision(18, 2);
                b.Property(b => b.Alert).HasDefaultValue(80);

                b.HasOne(b => b.User)
                    .WithMany(u => u.Budgets)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Category)
                    .WithMany(c => c.Budgets)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(b => b.UserId);
            });

            // RecurringTransaction configuration
            modelBuilder.Entity<RecurringTransaction>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.Description).IsRequired().HasMaxLength(500);
                b.Property(r => r.Amount).HasPrecision(18, 2);

                b.HasOne(r => r.User)
                    .WithMany(u => u.RecurringTransactions)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(r => r.Category)
                    .WithMany(c => c.RecurringTransactions)
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(r => r.UserId);
            });
        }
    }
}
