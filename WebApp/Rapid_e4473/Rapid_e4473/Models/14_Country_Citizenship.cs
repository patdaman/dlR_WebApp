using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _14a_Country_Citizenship
    {
        [Required(ErrorMessage = "Citizenship Required")]
        [Display(Name = "Citizenship")]
        public string CITIZEN { get; set; }
    }

    public class _14_Country_Citizenship
    {
        [Required(ErrorMessage = "Citizenship Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the citizenship country.")]
        [Display(Name = "Citizenship")]
        public string CITIZENSHIP { get; set; }

    }

    public class _14b_Country_Citizenship
    {
        [Required(ErrorMessage = "Citizenship Required")]
        [StringLength(50, ErrorMessage = "Citizenship country must be under 50 chars.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the citizenship country.")]
        [Display(Name = "Enter all that apply")]
        public string FOREIGNCITIZENSHIP { get; set; }

    }
}