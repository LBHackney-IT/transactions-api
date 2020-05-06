using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Helpers;
using transactions_api.V1.Validation;
using transactions_api.V1.Validation.ValidatorBase;
using UnitTests.V1.Helper;

namespace transactions_api.Tests.V1.Validation
{
    [TestFixture]
    public class GetTenancyTransactionsValidatorTests
    {
        private GetTenancyTransactionsValidator _validator;
        private Mock<IPostCodeBaseValidator> _postcodeBaseValidator;

        [SetUp]
        public void SetUp()
        {
            _postcodeBaseValidator = new Mock<IPostCodeBaseValidator>();
            _validator = new GetTenancyTransactionsValidator(_postcodeBaseValidator.Object);
        }

        #region Field Is Required [Null]

        [Test]
        public void given_a_request_with_null_PaymentRef_when_GetTenancyTransactionsValidator_is_called_then_it_returns_an_error()
        {
            //arrange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PaymentRef = null;

            //act, assert
            _validator.ShouldHaveValidationErrorFor(req => req.PaymentRef, request).WithErrorMessage(ErrorMessagesFormatter.FieldIsNullMessage("Payment reference"));
        }

        [Test]
        public void given_a_request_with_null_PostCode_when_GetTenancyTransactionsValidator_is_called_then_it_returns_an_error()
        {
            //arrange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = null;

            //act, assert
            _validator.ShouldHaveValidationErrorFor(req => req.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldIsNullMessage("Postcode"));
        }

        #endregion


        #region Field is required [Whitespace or Empty]

        [TestCase("")]
        [TestCase(" ")]
        public void given_a_request_with_empty_or_whitespace_PaymentRef_when_GetTenancyTransactionsValidator_is_called_then_it_returns_an_error(string paymentRef)
        {
            //arrange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PaymentRef = paymentRef;

            //act, assert
            _validator.ShouldHaveValidationErrorFor(req => req.PaymentRef, request).WithErrorMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Payment reference"));
        }

        [TestCase("")]
        [TestCase(" ")]
        public void given_a_request_with_empty_or_whitespace_PostCode_when_GetTenancyTransactionsValidator_is_called_then_it_returns_an_error(string postCode)
        {
            //arrange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;

            //act, assert
            _validator.ShouldHaveValidationErrorFor(req => req.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldIsWhiteSpaceOrEmpty("Postcode"));
        }

        #endregion

        #region Postcode format validation

        [Test]
        public void given_a_nonempty_request_when_GetTenancyTransactionsValidator_is_called_then_it_calls_PostCodeBaseValidator()       // non-empty because we want to stop on first failure.
        {
            var nonemptyRequest = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            _validator.Validate(nonemptyRequest);

            _postcodeBaseValidator.Verify(bv => bv.ValidatePostCodeFormat(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void given_a_nonempty_request_when_GetTenancyTransactionsValidator_is_called_then_it_calls_PostCodeBaseValidator_with_the_PostCode_from_the_request()
        {
            var nonemptyRequest = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            _validator.Validate(nonemptyRequest);

            _postcodeBaseValidator.Verify(bv => bv.ValidatePostCodeFormat(nonemptyRequest.PostCode), Times.Once);
        }

        [Test]
        public void given_a_nonempty_request_with_valid_PostCode_format_when_GetTenancyTransactionsValidator_is_called_then_it_returns_no_error()
        {
            //arange
            var nonemptyRequest = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            _postcodeBaseValidator.Setup(bv => bv.ValidatePostCodeFormat(It.IsAny<string>())).Returns(true);                            // setup to trigger successful validation

            //act, assert
            _validator.ShouldNotHaveValidationErrorFor(req => req.PostCode, nonemptyRequest);
        }

        [Test]
        public void given_a_nonempty_request_with_invalid_PostCode_format_when_GetTenancyTransactionsValidator_is_called_then_it_returns_an_error_with_correct_message()
        {
            var nonemptyRequest = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            _postcodeBaseValidator.Setup(bv => bv.ValidatePostCodeFormat(It.IsAny<string>())).Returns(false);                           // setup to trigger failed validation

            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, nonemptyRequest).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        #endregion

        #region Payment Ref validity tests

        [Test]
        public void given_a_request_with_valid_PaymentRef_when_GetTenancyTransactionsValidator_is_called_then_it_returns_no_error()
        {
            //arange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            //act, assert
            _validator.ShouldNotHaveValidationErrorFor(req => req.PaymentRef, request);
        }

        #endregion

    }
}
