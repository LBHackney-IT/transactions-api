using FluentValidation.Results;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Helpers;
using Bogus;

namespace transactions_api.Tests.V1.Helper
{
    [TestFixture]
    public class ErrorMessagesFormatterTests
    {
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _faker = new Faker();
        }

        [Test]
        public void given_a_list_of_validation_failures_when_FormatValidationFailures_error_formatter_method_is_called_then_it_returns_a_list_of_corresponding_error_messages() //we don't check the message format. we only check that the related text was given as output.
        {
            //arrange
            int errorCount = _faker.Random.Int(1, 10);                                                                                                     //simulate from 1 to 10 validation errors (triangulation).
            var validationFailuresList = new List<ValidationFailure>();                                                                                    //this list will be used as constructor argument for 'ValidationResult'.
            for (int i = errorCount; i > 0; i--) { validationFailuresList.Add(new ValidationFailure(_faker.Random.Word(), _faker.Random.Word())); }        //generate from 1 to 10 fake validation errors. Single line for-loop so that it wouldn't distract from what's key in this test.

            //act
            var formattedList = ErrorMessagesFormatter.FormatValidationFailures(validationFailuresList);

            //assert
            Assert.AreEqual(errorCount, formattedList.Count);

            for(int i = 0; i < errorCount ; i++)
            {
                StringAssert.Contains(validationFailuresList[i].PropertyName, formattedList[i]);
                StringAssert.Contains(validationFailuresList[i].ErrorMessage, formattedList[i]);
            }
        }
    }
}
