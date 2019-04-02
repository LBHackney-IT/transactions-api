using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dt = (DateTime)value;
            if (dt <= DateTime.Now)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult
                ("The date specified must not be a future date");
        }
    }
}
