using FluentValidation.Results;
using transactions_api.V1.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Validation
{
    public interface IGetTenancyDetailsValidator
    {
        ValidationResult Validate(GetTenancyDetailsRequest request);
    }
}
