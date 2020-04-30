using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using transactions_api.V1.Boundary;
using transactions_api.V1.Helpers;

namespace transactions_api.V1.Validation
{
    public class GetTenancyDetailsValidator : AbstractValidator<GetTenancyDetailsRequest>, IGetTenancyDetailsValidator
    {
        public GetTenancyDetailsValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(request => request.PaymentRef)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Payment reference"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Payment reference"));

            RuleFor(request => request.PostCode)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Postcode"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Postcode"))
                .Matches(new Regex("^((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]))))( )?(([0-9][A-Za-z]?[A-Za-z]?)?))$")) //TODO: change the regex not to accept partial postcodes.
                .WithMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }
    }
}
