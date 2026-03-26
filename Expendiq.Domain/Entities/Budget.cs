using Expendiq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expendiq.Domain.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal LimitAmount { get; set; }
        public decimal Alert { get; set; } // Alert at this percentage (e.g., 80 for 80%)
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public BudgetPeriod Period { get; set; } = BudgetPeriod.Monthly;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Category? Category { get; set; }
    }

   
}