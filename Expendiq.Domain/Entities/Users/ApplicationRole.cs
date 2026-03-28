using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expendiq.Domain.Entities.Users
{
    [Table("AspNetRoles", Schema = "idn")]
    public class ApplicationRole : IdentityRole<int>
    {
        [Required]
        [Column(Order = 101)]
        public bool IsActive { get; set; }

        [Required]
        [Column(Order = 102)]
        public int EntryUserID { get; set; }

        [Required]
        [Column(Order = 103, TypeName = "date")]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

    }
}
