using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _10a_Ethnicity
    {
        [Required(ErrorMessage = "Ethnicity Required")]
        [RegularExpression(@"^[HhNn\s]{1,40}$",
            ErrorMessage = "Ethnicity must be 'H' or 'N'.")]
        [StringLength(1, ErrorMessage = "Ethnicity can only be 1 char.")]
        [Display(Name = "Choose One")]
        public string ETHNICITY { get; set; }
    }

    public class _10b_Race
    {
        [Display(Name = "Check one or more")]
        public string RACE { get; set; }

        public bool RACE_A { get; set; }
        public bool RACE_N { get; set; }
        public bool RACE_B { get; set; }
        public bool RACE_H { get; set; }
        public bool RACE_W { get; set; }
    }
}