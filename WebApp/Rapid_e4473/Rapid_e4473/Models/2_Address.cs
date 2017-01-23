using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using Rapid_e4473.HelperClasses;

namespace Rapid_e4473.Models
{
    public class _2_Address
    {
        [Required(ErrorMessage = "Address Required")]
        [RegularExpression(@"^(?i)(?!p\.?o\.?\sbox|post\soffice).*^[^|]+$",
            ErrorMessage = "PO Boxes and '|' are not allowed.")]
        [StringLength(100, ErrorMessage = "Address must be under 100 chars.")]
        [Display(Name = "Address")]
        public string ADDRS_1 { get; set; }

        [RegularExpression(@"^(?i)(?!p\.?o\.?\sbox|post\soffice).*^[^|]+$",
            ErrorMessage = "PO Boxes and '|' are not allowed.")]
        [StringLength(100, ErrorMessage = "Address 2 must be under 100 chars.")]
        [Display(Name = "Address Extended (optional)")]
        public string ADDRS_2 { get; set; }

        [Required(ErrorMessage = "City Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the city name.")]
        [StringLength(50, ErrorMessage = "City must be under 50 chars.")]
        [Display(Name = "City")]
        public string CITY { get; set; }

        [Required(ErrorMessage = "County Required")]
        [CountyAttribute]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the county name.")]
        [StringLength(50, ErrorMessage = "County must be under 50 chars.")]
        [Display(Name = "County")]
        public string COUNTY { get; set; }

        [Required(ErrorMessage = "State Required")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
            ErrorMessage = "Numbers and special characters are not allowed in the state name.")]
        [StringLength(10, ErrorMessage = "State must be under 10 chars.")]
        [Display(Name = "State")]
        public string STATE { get; set; }

        [Required(ErrorMessage = "Zip Code Required")]
        [RegularExpression(@"^(\d{5}(?:\-\d{4})?)$",
            ErrorMessage = "Zip code must match 5 digit (XXXXX) or 5+4 digit (XXXXX-XXXX) format.")]
        [StringLength(10, ErrorMessage = "Zip Code must be under 10 chars.")]
        [Display(Name = "Zip Code")]
        public string ZIP_COD { get; set; }

    }
}