using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Expendiq.Application.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [Display(Name = "Username or Email")]
        public string UserName { get; set; }

        public string ReturnUrl { get; set; }
    }
}
