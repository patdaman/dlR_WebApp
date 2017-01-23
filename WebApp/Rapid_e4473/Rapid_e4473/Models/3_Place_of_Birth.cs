using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _3a_Place_of_Birth
    {
        [Required(ErrorMessage="Country of Birth Required")]
        [Display(Name = "Born in USA?")]
        public string PLACE_OF_BIRTH_CNFRM { get; set; }
    }

    public class _3b_Place_of_Birth
    {
        [Required(ErrorMessage = "City of Birth Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the US place of birth.")]
        [StringLength(100, ErrorMessage = "City of birth must be under 50 chars.")]
        [Display(Name = "City of Birth - City")]
        public string PLACE_OF_BIRTH_CITY { get; set; }

        [Required(ErrorMessage = "State of Birth Required")]
        [Display(Name = "State of Birth")]
        public string PLACE_OF_BIRTH_STATE { get; set; }
    }

    public class _3c_Place_of_Birth
    {
        [Required(ErrorMessage = "Place of Birth Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the foreign place of birth.")]
        [StringLength(100, ErrorMessage = "Place of birth must be under 50 chars.")]
        [Display(Name = "Foreign Country of Birth")]
        public string PLACE_OF_BIRTH_FOREIGN { get; set; }
    }
}