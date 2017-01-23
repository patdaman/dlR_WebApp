using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _12_Alien_Status
    {
        [RegularExpression(@"^[YyNn\s]{1,40}$",
            ErrorMessage = "Answer must be 'Y', 'N', or left blank.")]
        [StringLength(1, ErrorMessage = "#12 can only be 1 char.")]
        [Display(Name = "Alien Status")]
        public string ALIEN { get; set; }
    }
}