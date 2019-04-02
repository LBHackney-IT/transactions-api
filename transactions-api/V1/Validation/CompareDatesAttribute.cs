using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Boundary;

namespace transactions_api.V1.Validation
{
    public class CompareDatesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ListTransactionsRequest)validationContext.ObjectInstance;
            var fromDate = (DateTime)value;
            //if user did not pass toDate, we should look at DateTime.Today
            var toDate = model.toDate == DateTime.MinValue ? DateTime.Today : model.toDate;
            //if to date is before from date, fail validation
            if (toDate < fromDate)
            {
                return new ValidationResult
                    ("The start date must be earlier than the end date");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
