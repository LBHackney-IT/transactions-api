using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using FV = FluentValidation.Results;
using Bogus;
using transactions_api.V1.Exceptions;
using NUnit.Framework;
using transactions_api.V1.Helpers;

namespace transactions_api.Tests.V1.Exceptions
{
    [TestFixture]
    public class ErrorResponseTests
    {
        private Faker _faker = new Faker();

        [Test]
        public void given_no_parameters_when_ErrorResponse_constructor_is_called_then_it_initializes_errors_parameter_to_empty_list()
        {
            //act
            var errorResponse = new ErrorResponse();

            //assert
            Assert.NotNull(errorResponse.errors);
            Assert.Zero(errorResponse.errors.Count);
        }

        [Test]
        public void given_a_list_of_error_strings_when_ErrorResponse_constructor_is_called_then_it_initializes_errors_parameter_to_passed_in_list_of_errors()
        {
            //arrange
            var expectedListOfErrors = new List<string>();

            for (int i = _faker.Random.Int(1, 10); i > 0; i--)
                expectedListOfErrors.Add(_faker.Random.Hash());

            //act
            var errorResponse = new ErrorResponse(expectedListOfErrors);
            var actualListOfErrors = errorResponse.errors;

            //assert
            Assert.NotNull(actualListOfErrors);
            Assert.AreSame(expectedListOfErrors, actualListOfErrors); //they should have the same obj reference
        }

        [Test]
        public void given_any_number_of_error_string_parameters_when_ErrorResponse_constructor_is_called_then_it_initializes_errors_parameter_to_a_list_of_error_strings_corresponding_to_passed_in_parameters()
        {
            //act
            var singleParameterErrorResponse = new ErrorResponse( _faker.Random.Word() );
            var fourParameterErrorResponse = new ErrorResponse( _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word() );
            var sevenParameterErrorResponse = new ErrorResponse( _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word(), _faker.Random.Word() );

            //assert
            Assert.AreEqual(1, singleParameterErrorResponse.errors.Count);
            Assert.AreEqual(4, fourParameterErrorResponse.errors.Count);
            Assert.AreEqual(7, sevenParameterErrorResponse.errors.Count);
        }

        [Test]
        public void given_a_list_of_validation_failures_when_ErrorResponse_constructor_is_called_then_it_initializes_errors_parameter_to_a_list_of_error_messages()
        {
            //arrange
            int errorCount = _faker.Random.Int(1, 10);                                                                                                     //simulate from 1 to 10 validation errors (triangulation).
            var validationFailuresList = new List<ValidationFailure>();                                                                                    //this list will be used as constructor argument for 'ValidationResult'.
            for (int i = errorCount; i > 0; i--) { validationFailuresList.Add(new ValidationFailure(_faker.Random.Word(), _faker.Random.Word())); }        //generate from 1 to 10 fake validation errors. Single line for-loop so that it wouldn't distract from what's key in this test.

            //act
            var errorResponse = new ErrorResponse(validationFailuresList);

            //assert
            Assert.AreEqual(errorCount, errorResponse.errors.Count);
        }
    }
}