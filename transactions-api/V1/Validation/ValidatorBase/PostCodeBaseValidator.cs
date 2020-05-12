using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace transactions_api.V1.Validation.ValidatorBase
{
    public class PostCodeBaseValidator : IPostCodeBaseValidator
    {
        public bool ValidatePostCodeFormat(string postcode)
        {
            var postcodeFormatPattern = new Regex("^[A-Za-z]{1,2}[0-9][A-Za-z0-9]? ?[0-9][A-Za-z]{2}$");

            var isFormatValid = postcodeFormatPattern.IsMatch(postcode);

            return isFormatValid;
        }
    }
}
