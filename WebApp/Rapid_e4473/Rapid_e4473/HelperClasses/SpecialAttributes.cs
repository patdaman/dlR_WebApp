using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Rapid_e4473.HelperClasses
{
    public class CountyAttribute : RegularExpressionAttribute
    {
        public CountyAttribute()
            : base("(?!^USA|US|United States|United States of America|usa|us$).*")
        {
            this.ErrorMessage = "Please provide a valid COUNTY of residence.";
        }
    }

    public class DateRangeAttribute : ValidationAttribute
    {
        public int FirstDateYears { get; set; }
        public int SecondDateYears { get; set; }

        public DateRangeAttribute()
        {
            FirstDateYears = 110;
            SecondDateYears = 18;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            String valueString = value.ToString();
            DateTime date = DateTime.Parse(valueString);

            if ((date >= DateTime.Now.AddYears(-FirstDateYears)) &&
                (date <= DateTime.Now.AddYears(-SecondDateYears)))
                return true;

            return false;

        }
    }
}