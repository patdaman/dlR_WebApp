using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _13_State_of_Residence
    {
        [Required(ErrorMessage = "State of Residence Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the state name.")]
        [StringLength(10, ErrorMessage = "State of residence must be under 10 chars.")]
        [Display(Name = "Residence State")]
        public string RESIDENCE_STATE { get; set; }
    }
}