using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.Models
{
    public class _11a
    {
        [Required(ErrorMessage="Answer is required.")]
        [RegularExpression(@"^[YyNnRr]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11A { get; set; }
    }

    public class _11b
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11B { get; set; }
    }

    public class _11c
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11C { get; set; }
    }

    public class _11d
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11D { get; set; }
    }

    public class _11e
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11E { get; set; }
    }

    public class _11f
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11F { get; set; }
    }

    public class _11g
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11G { get; set; }
    }

    public class _11h
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11H { get; set; }
    }

    public class _11i
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11I { get; set; }
    }

    public class _11j
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11J { get; set; }
    }

    public class _11k
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11K { get; set; }
    }

    public class _11l
    {
        [Required(ErrorMessage = "Answer is Required")]
        [RegularExpression(@"^[YyNn]{1,40}$",
            ErrorMessage = "Answer must be 'Y' or 'N'.")]
        [StringLength(1, ErrorMessage = "#11 can only be 1 char.")]
        public string C11L { get; set; }
    }
}