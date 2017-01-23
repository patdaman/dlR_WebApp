using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _9_UPIN
    {
        [RegularExpression(@"^[a-zA-Z0-9#*()@''-'\s]{1,40}$",
            ErrorMessage = "Pipes ('|') are not allowed in UPIN.")]
        [StringLength(50, ErrorMessage = "UPIN must be under 50 chars.")]
        [Display(Name = "UPIN")]
        public string UPIN { get; set; }
    }
}