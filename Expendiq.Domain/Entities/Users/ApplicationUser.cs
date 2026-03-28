using Expendiq.Domain.Enums.User;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expendiq.Domain.Entities.Users
{
    [Table("AspNetUsers", Schema = "idn")]
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [Column(Order = 100, TypeName = "varchar(100)")]
        public string FullName { get; set; }

        [Required]
        [Column(Order = 102, TypeName = "varchar(20)")]
        public string MobileNumber { get; set; }

        [Required]
        [Column(Order = 103)]
        public int EntryUserID { get; set; }

        [Required]
        [Column(Order = 104, TypeName = "date")]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(Order = 105)]
        public UserStatus Status { get; set; }
    }
}
