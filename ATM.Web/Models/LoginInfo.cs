using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Models
{
    public class LoginInfo : BaseView
    {
        [Required]
        [Display(Name = "Card No.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Please provide a valid card number")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Only numeric values are allowed")]

        public string LoginId { get; set; }
        [Required]
        [Display(Name = "Pin")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Please provide a valid Pin")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Only numeric values are allowed")]
        public string Password { get; set; }
    }


}
