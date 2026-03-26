using Expendiq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expendiq.Domain.Entities
{
    public class RecurringTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.Monthly;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime LastProcessedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Category? Category { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
