using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using Rapid_e4473.HelperClasses;

namespace Rapid_e4473.Models
{
    public class _7_Birth_Date
    {
        [Required(ErrorMessage = "Birth day Required")]
        [DataType(DataType.Date)]
        [DateRange(ErrorMessage = "Must be over 18 years old.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthdate")]
        public Nullable<DateTime> BIRTHDATE { get; set; }
    }
}