using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _8_Social_Security
    {
        [RegularExpression(@"^\d{9}|\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid Social Security Number")]
        //[RegularExpression(@"^[0-9\s-]{1,40}$",
        //    ErrorMessage = "Letters and pipes are not allowed in the height.")]
        [StringLength(12, ErrorMessage = "Social Security # must be under 12 chars.")]
        [Display(Name = "Social Security #")]
        public string SOC_SEC_NUM { get; set; }
    }
}