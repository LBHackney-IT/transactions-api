using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using transactions_api.V1.Boundary;
using transactions_api.V1.Helpers;
using transactions_api.V1.Validation.ValidatorBase;

namespace transactions_api.V1.Validation
{
    public class GetTenancyDetailsValidator : AbstractValidator<GetTenancyDetailsRequest>, IGetTenancyDetailsValidator
    {
        private readonly IPostCodeBaseValidator _postcodeBaseValidator;

        public GetTenancyDetailsValidator(IPostCodeBaseValidator postcodeBaseValidator)
        {
            #region Injecting dependencies

            _postcodeBaseValidator = postcodeBaseValidator;

            #endregion

            ValidatorOptions.Global.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(request => request.PaymentRef)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Payment reference"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Payment reference"));

            RuleFor(request => request.PostCode)
                .NotNull().WithMessage(ErrorMessagesFormatter.FieldIsNullMessage("Postcode"))
                .NotEmpty().WithMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Postcode"))
                .Must(_postcodeBaseValidator.ValidatePostCodeFormat)
                .WithMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }
    }
}
