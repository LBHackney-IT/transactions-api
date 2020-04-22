using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using FV = FluentValidation.Results;

namespace transactions_api.V1.Helpers
{
    /// <summary>
    /// The idea behind this is having a centralized place to store error message formats.
    /// 
    /// Issue is that whenever there is a change to the error message or the way it's phrased, you have to go
    /// about changing it at different places in code where it appears - not only in implementation from endpoint to endpoint,
    /// but tests as well.
    /// 
    /// Rather than doing that, I think it's for the best to have a helper method for the error messages, and then
    /// if there's a need for a change something, you do it here in one spot. This also has the benefit of improved consistency.
    /// </summary>

    public static class ErrorMessagesFormatter
    {
        public static List<string> FormatValidationFailures(IList<ValidationFailure> validationFailures)
        {
            return validationFailures.Aggregate(
                            new List<string>(),
                            (errorList, validationFailure) =>
                            {
                                errorList.Add(FluentValidationsFailureMessage(validationFailure.PropertyName, validationFailure.ErrorMessage));
                                return errorList;
                            }
                        );
        }

        public static string FluentValidationsFailureMessage(string propertyName, string errorMessage)
        {
            return $"Property {propertyName} failed validation. Error was: {errorMessage}";
        }

        public static string FieldIsNullMessage(string fieldName)
        {
            return $"{fieldName} must be provided.";
        }

        public static string FieldIsWhiteSpaceOrEmpty(string fieldName)
        {
            return $"{fieldName} must not be empty.";
        }

        public static string FieldWithIncorrectFormat(string fieldName)
        {
            return $"Provided {fieldName} format is incorrect.";
        }
    }
}
