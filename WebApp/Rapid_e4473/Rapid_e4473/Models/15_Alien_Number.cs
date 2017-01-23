using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _15_Alien_Number
    {
        [Required(ErrorMessage="Alien number required for non-US citizens")]
        [StringLength(50, ErrorMessage = "Alien number must be under 50 chars.")]
        [RegularExpression(@"^[a-zA-Z0-9#*()@''-'\s]{1,40}$",
            ErrorMessage = "Pipes ('|') not allowed in the Alien Number.")]
        [Display(Name = "Alien Number")]
        public string ALIEN_NUM { get; set; }
    }
}