using Expendiq.Domain.Entities.Users;

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

        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? RecurringTransactionId { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public RecurringTransaction? RecurringTransaction { get; set; }
    }

}
