using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expendiq.Application.ViewModels
{
    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "Register Type is required.")]
        //public RegisterType RegisterType { get; set; }

        [Display(Name = "Agent Code")]
        public string AgentCode { get; set; }

        public int AgentId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public int BranchId { get; set; }

        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public string ReturnUrl { get; set; }

    }
}
