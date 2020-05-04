using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Helpers;
using transactions_api.V1.Validation;
using UnitTests.V1.Helper;

namespace transactions_api.Tests.V1.Validation
{
    [TestFixture]
    public class GetTenancyTransactionsValidatorTests
    {
        private GetTenancyTransactionsValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new GetTenancyTransactionsValidator();
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

        // Below explanations all use the postcodes IG11 7QD and E5 3XW 
        //"Incode" refers to the whole second part of the postcode (i.e. 3XW, 7QD) from (E11 3XW, W3 7QD)
        //"Outcode" refers to the whole first part of the postcode (Letter(s) and number(s) - i.e. IG11, E5) from (IG11 9LL, E5 2LL)
        //"Area" refers to the first letter(s) of the postcode (i.e.  IG, E) from (IG11 9LL, E5 2LL)
        //"District" refers to first number(s) to appear in the postcode (i.e. 11, 5) from (IG11 9LL, E5 2LL)
        //"Sector" refers to the number in the second part of the postcode (i.e. 9, 7) from (SW2 9DN, NE4 7JU)
        //"Unit" refers to the letters in the second part of the postcode (i.e. DN, JU) from (SW2 9DN, NE4 7JU)

        [TestCase("CR1 3ED")]
        public void GivenAPostCodeValueInUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("w2 5jq")]
        public void GivenAPostCodeValueInLowerCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("w2 5JQ")]
        [TestCase("E11 5ra")]
        public void GivenAPostCodeValueInLowerCaseAndUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("CR13ED")]
        [TestCase("RE15AD")]
        public void GivenPostCodeValueWithoutSpaces_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("NW")]
        [TestCase("E")]
        public void GivenOnlyAnAreaPartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("17 9LL")]
        [TestCase("8 1LA")]
        public void GivenOnlyAnIncodeAndADistrictPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("NW 9LL")]
        [TestCase("NR1LW")]
        public void GivenOnlyAnIncodeAndAnAreaPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("1LL")]
        [TestCase(" 6BQ")]
        public void GivenOnlyAnIncodePartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("E8 1LL")]
        [TestCase("SW17 1JK")]
        public void GivenBothPartsOfPostCode_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("IG117QDfdsfdsfd")]
        [TestCase("E1llolol")]
        public void GivenAValidPostcodeFolowedByRandomCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("EEE")]
        [TestCase("THE")]
        public void GivenThreeCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("N8 LL")]
        [TestCase("NW11 AE")]
        public void GivenAnOutcodeAndAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        [TestCase("S10 H")]
        [TestCase("W1 J")]
        public void GivenAnOutcodeAndOnlyOneLetterOfAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();
            request.PostCode = postCode;
            _validator.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage(ErrorMessagesFormatter.FieldWithIncorrectFormat("postcode"));
        }

        #endregion

        #region format is valid tests

        [Test]
        public void given_a_request_with_valid_PaymentRef_when_GetTenancyTransactionsValidator_is_called_then_it_returns_no_error()
        {
            //arange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            //act, assert
            _validator.ShouldNotHaveValidationErrorFor(req => req.PaymentRef, request);
        }

        [Test]
        public void given_a_request_with_valid_PostCode_when_GetTenancyTransactionsValidator_is_called_then_it_returns_no_error() //if this ever fails, it's probably because faker isn't generating correct code
        {
            //arange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            //act, assert
            _validator.ShouldNotHaveValidationErrorFor(req => req.PostCode, request);
        }

        #endregion

    }
}
