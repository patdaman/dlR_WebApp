using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _1a_Name
    {
        [Key]
        public int CUST_ID { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [StringLength(50, ErrorMessage = "First Name must be under 50 chars.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the first name.")]
        [Display(Name = "First Name")]
        public string FIRST_NAME { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the last name.")]
        [StringLength(50, ErrorMessage = "Last Name must be under 50 chars.")]
        [Display(Name = "Last Name")]
        public string LAST_NAME { get; set; }
    }

    public class _1b_Name
    {
        [Required(ErrorMessage = "Middle Name Answer Required.")]
        [Display(Name = "Do you have a middle name?")]
        public string MIDDLE_NAME_CNFRM { get; set; }
    }

    public class _1c_Name
    {
        [Required(ErrorMessage = "Middle Name Required")]
        [StringLength(30, ErrorMessage = "Middle Name must be under 30 chars.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the middle name.")]
        [Display(Name = "Middle Name")]
        public string MIDDLE_NAME { get; set; }
    }
}