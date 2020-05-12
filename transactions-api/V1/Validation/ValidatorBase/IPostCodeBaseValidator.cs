using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Validation.ValidatorBase
{
    /// <summary>
    /// The implementation should contain methods related to postcode validation.
    /// In this case it would be postcode format validation (and postcode characters validation - if needed later on).
    /// </summary>
    public interface IPostCodeBaseValidator
    {
        bool ValidatePostCodeFormat(string postcode);
    }
}
