using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _5_Weight
    {
        [Required(ErrorMessage = "Weight Required")]
        [Range(50, 1000, ErrorMessage = "Weight must be between 50 and 1000 lbs.")]
        [Display(Name = "Weight (lbs.)")]
        public string WEIGHT { get; set; }
    }
}