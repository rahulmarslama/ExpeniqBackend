using System;
using System.Collections.Generic;
using System.Text;

namespace Expendiq.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public bool IsRecurring { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? RecurringTransactionId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Category? Category { get; set; }
        public RecurringTransaction? RecurringTransaction { get; set; }
    }

}
