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
    public class GetTenancyTransactionsValidator : AbstractValidator<GetAllTenancyTransactionsRequest>, IGetTenancyTransactionsValidator
    {
        public GetTenancyTransactionsValidator()
        {
            RuleFor(request => request.PaymentRef)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Payment reference"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Payment reference"));

            RuleFor(request => request.PostCode)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Postcode"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Postcode"))
                .Matches(new Regex("^((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]))))( )?(([0-9][A-Za-z]?[A-Za-z]?)?))$"))
                .WithMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }
    }
}
