using Bogus;
using FluentValidation.Results;
using FV = FluentValidation.Results;
using System.Collections.Generic;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Helper
{
    public static class TransactionHelper
    {
        private static Faker _faker = new Faker("en_GB");

        public static Transaction CreateTransaction()
        {
            var transaction = new Transaction
            {
                Date = _faker.Date.Past(),
                Code = _faker.Random.Hash(3),
                Description = _faker.Random.Hash(15),
                Amount = _faker.Finance.Amount(),
                Comments = _faker.Random.Hash(15),
                FinancialYear = _faker.Date.Past().Year,
                PeriodNumber = _faker.Random.Int(0,99),
                RunningBalance = _faker.Finance.Amount()
            };
            return transaction;
        }

        public static GetAllTenancyTransactionsRequest CreateGetAllTenancyTransactionsRequestObject()
        {
            return new GetAllTenancyTransactionsRequest()
            {
                PaymentRef = _faker.Random.Hash(),
                PostCode = _faker.Address.ZipCode()
            };
        }


        #region Fake Validation Results

        public static List<ValidationFailure> GenerateAListOfValidationFailures()
        {
            int errorCount = _faker.Random.Int(1, 10);                                                                                                  //simulate from 1 to 10 validation errors (triangulation).
            var validationErrorList = new List<ValidationFailure>();                                                                                    //this list will be used as constructor argument for 'ValidationResult'.
            for (int i = errorCount; i > 0; i--) { validationErrorList.Add(new ValidationFailure(_faker.Random.Word(), _faker.Random.Word())); }        //generate from 1 to 10 fake validation errors. Single line for-loop so that it wouldn't distract from what's key in this test.

            return validationErrorList;
        }

        public static FV.ValidationResult GenerateFailedValidationResult()
        {
            var validationErrorList = GenerateAListOfValidationFailures();
            return new FV.ValidationResult(validationErrorList);                                                                                        //Need to create ValidationResult so that I could setup Validator mock to return it upon '.Validate()' call. Also this is the only place where it's possible to manipulate the validation result - You can only make the validation result invalid by inserting a list of validation errors as a parameter through a constructor. The boolean '.IsValid' comes from expression 'IsValid => Errors.Count == 0;', so it can't be set manually.
        }

        public static FV.ValidationResult GenerateSuccessValidationResult()
        {
            return new FV.ValidationResult();                                                                                                           //This is a success, because no validation failures were passed into the constructor. The boolean '.IsValid' comes from expression 'IsValid => Errors.Count == 0;', so it can't be set manually.
        }

        #endregion
    }
}
