using System;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class LoginViewModel
    {
        [Required]
        [Range(999999999, 9999999999, ErrorMessage = "Barcode is 10 digits")]
        [Display(Name = "Barcode")]
        public string BARCODE { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PASSWORD { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EMAIL { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EMAIL { get; set; }
    }

    public class submit
    {
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{5,}", ErrorMessage = "Your password must be at least 5 characters long and contain at least 1 letter and 1 number")]
        public string PASSWORD { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("PASSWORD", ErrorMessage = "The password and confirmation password do not match.")]
        public string CONFIRMPASSWORD { get; set; }
    }

    public class CustomerDTO
    {
        public int CUST_ID { get; set; }
        public string EMAIL { get; set; }
        public string BARCODE { get; set; }
        public string PASSWORD { get; set; }
        public Nullable<System.DateTime> TIMESTAMP { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string ADDRS_1 { get; set; }
        public string ADDRS_2 { get; set; }
        public string CITY { get; set; }
        public string COUNTY { get; set; }
        public string STATE { get; set; }
        public string ZIP_COD { get; set; }
        public string PLACE_OF_BIRTH_CITY { get; set; }
        public string PLACE_OF_BIRTH_STATE { get; set; }
        public string PLACE_OF_BIRTH_FOREIGN { get; set; }
        public string HEIGHT_FT { get; set; }
        public string HEIGHT_IN { get; set; }
        public string WEIGHT { get; set; }
        public string GENDER { get; set; }
        public string BIRTHDATE { get; set; }
        public string SOC_SEC_NUM { get; set; }
        public string UPIN { get; set; }
        public string ETHNICITY { get; set; }
        public string RACE { get; set; }
        public string C11A { get; set; }
        public string C11B { get; set; }
        public string C11C { get; set; }
        public string C11D { get; set; }
        public string C11E { get; set; }
        public string C11F { get; set; }
        public string C11G { get; set; }
        public string C11H { get; set; }
        public string C11I { get; set; }
        public string C11J { get; set; }
        public string C11K { get; set; }
        public string C11L { get; set; }
        public string ALIEN { get; set; }
        public string RESIDENCE_STATE { get; set; }
        public string CITIZENSHIP { get; set; }
        public string ALIEN_NUM { get; set; }
    }
}
