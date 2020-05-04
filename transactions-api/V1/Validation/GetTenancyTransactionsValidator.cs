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
            ValidatorOptions.Global.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(request => request.PaymentRef)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Payment reference"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Payment reference"));

            RuleFor(request => request.PostCode)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Postcode"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Postcode"))
                .Matches(new Regex("^[A-Za-z]{1,2}[0-9][A-Za-z0-9]? ?[0-9][A-Za-z]{2}$"))
                .WithMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }
    }
}
