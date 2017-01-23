using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _4_Height
    {
        [Required(ErrorMessage = "Height Required")]
        [RegularExpression(@"^[0-9]{1,40}$",
            ErrorMessage = "Letters and characters are not allowed in the height.")]
        [StringLength(10, ErrorMessage = "Height must be under 10 chars.")]
        [Range(4, 8, ErrorMessage = "Height must be between 4 - 7 feet.")]
        [Display(Name = "Height - feet")]
        public string HEIGHT_FT { get; set; }

        [Required(ErrorMessage = "Height Required")]
        [RegularExpression(@"^[0-9]{1,40}$",
            ErrorMessage = "Letters and characters are not allowed in the height.")]
        [Range(0, 12, ErrorMessage = "Height must be between 0 - 12 inches.")]
        [StringLength(10, ErrorMessage = "Height must be under 10 chars.")]
        [Display(Name = "Height - inches")]
        public string HEIGHT_IN { get; set; }
    }
}