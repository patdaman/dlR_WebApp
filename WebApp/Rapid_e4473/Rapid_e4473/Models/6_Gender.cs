using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _6_Gender
    {
        [Required(ErrorMessage = "Gender Required")]
        [Display(Name = "Gender")]
        public string GENDER { get; set; }
    }
}