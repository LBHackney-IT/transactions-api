using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using transactions_api.Controllers.V1;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;
using transactions_api.V1.Validation;
using UnitTests.V1.Helper;
using FluentValidation.Results;
using FV = FluentValidation.Results;
using transactions_api.V1.Exceptions;

namespace UnitTests.V1.Controllers
{

    [TestFixture]
    public class TransactionControllerTests
    {
        private TransactionsController _classUnderTest;

        private Mock<IListTransactions> _mockListTransacionsUsecase;
        private Mock<IGetTenancyTransactionsValidator> _mockGetTenancyTransactionsValidator;

        private Faker _faker = new Faker("en_GB");

        [SetUp]
        public void SetUp()
        {
            _mockListTransacionsUsecase = new Mock<IListTransactions>();
            _mockGetTenancyTransactionsValidator = new Mock<IGetTenancyTransactionsValidator>();
            ILogger<TransactionsController> nullLogger = NullLogger<TransactionsController>.Instance;
            _classUnderTest = new TransactionsController(_mockListTransacionsUsecase.Object, nullLogger, _mockGetTenancyTransactionsValidator.Object);
        }

        #region Transactions in General

        [Test]
        public void ReturnsCorrectRandomResponseWithStatus()
        {
            var transaction = TransactionHelper.CreateTransaction();
            var request = ListTransactionsRequest();
            var datetime = _faker.Date.Past();

            _mockListTransacionsUsecase.Setup(s =>
                    s.Execute(It.IsAny<ListTransactionsRequest>()))
                .Returns(new ListTransactionsResponse(new List<Transaction>{ transaction }, request, datetime));

            var response = _classUnderTest.GetTransactions(request);
            var json = JsonConvert.SerializeObject(response.Value);

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                { "request", new Dictionary<string, object>
                    {
                        {"tagRef", request.TagRef},
                        {"fromDate", request.fromDate},
                        {"toDate", request.toDate }
                    }
                },
                { "generatedAt", datetime},
                { "transactions", new [] { new Dictionary<string, object>
                        {
                            {"date", transaction.Date},
                            {"financialYear", transaction.FinancialYear },
                            {"periodNumber", transaction.PeriodNumber },
                            {"code", transaction.Code},
                            {"description", transaction.Description},
                            {"comments", transaction.Comments},
                            {"amount", transaction.Amount },
                            {"runningBalance", transaction.RunningBalance}
                        }
                    }
                }
            }), json);
        }

        private static ListTransactionsRequest ListTransactionsRequest()
        {
            var _faker = new Faker();
            var listTransactionsRequest = new ListTransactionsRequest
            {
                TagRef = _faker.Random.Hash(9),
                fromDate = _faker.Date.Past(),
                toDate = DateTime.Now
            };
            return listTransactionsRequest;
        }
        
        [Test]
        public void ReturnsCorrectResponseWithStatus()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2019, 02, 22, 09, 52, 23, 22),
                Code = "Field",
                Description = "Description",
                Amount = 35.0m,
                Comments = "Comments",
                FinancialYear = 2017,
                PeriodNumber = 3,
                RunningBalance = 508.64m
            };
            var request = new ListTransactionsRequest
            {
                TagRef = "testString",
                fromDate = DateTime.Parse("20-03-2018"),
                toDate = DateTime.Parse("20-03-2019")
            };

            var generatedAt = new DateTime(2019, 02, 22, 09, 52, 23, 23);
            _mockListTransacionsUsecase.Setup(s =>
                    s.Execute(It.IsAny<ListTransactionsRequest>()))
                .Returns(new ListTransactionsResponse(new List<Transaction>{ transaction }, request, generatedAt));

            var response = _classUnderTest.GetTransactions(request);
            var json = JsonConvert.SerializeObject(response.Value);

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(expectedJSON(), json);
        }

        private string expectedJSON()
        {
            string json =
@"{
  ""request"": {
    ""tagRef"": ""testString"",
    ""fromDate"": ""2018-03-20T00:00:00Z"",
    ""toDate"": ""2019-03-20T00:00:00Z""
  },
  ""generatedAt"": ""2019-02-22T09:52:23.023Z"",
  ""transactions"": [
    {
      ""date"": ""2019-02-22T09:52:23.022Z"",
      ""financialYear"": 2017,
      ""periodNumber"": 3.0,
      ""code"": ""Field"",
      ""description"": ""Description"",
      ""comments"": ""Comments"",
      ""amount"": 35.0,
      ""runningBalance"": 508.64
    }
  ]
}";
            return json;
        }

        #endregion

        #region Tenancy Transactions

        [Test]
        public void given_a_request_object_when_GetAllTenancyTransactions_controller_method_is_called_then_it_calls_the_validator_with_that_request_object()
        {
            //arrange
            var request = TransactionHelper.CreateGetAllTenancyTransactionsRequestObject();

            _mockGetTenancyTransactionsValidator.Setup(x => x.Validate(It.IsAny<GetAllTenancyTransactionsRequest>())).Returns(new FV.ValidationResult()); //setup validator to return a no error validation result

            //act
            _classUnderTest.GetAllTenancyTransactions(request);

            //assert
            _mockGetTenancyTransactionsValidator.Verify(v => v.Validate(It.Is<GetAllTenancyTransactionsRequest>(obj => obj == request)), Times.Once);
        }

        [Test]
        public void given_a_valid_request_when_GetAllTenancyTransactions_controller_method_is_called_then_it_returns_200_Ok_result()
        {
            //arrange
            var expectedStatusCode = 200;
            _mockGetTenancyTransactionsValidator.Setup(v => v.Validate(It.IsAny<GetAllTenancyTransactionsRequest>())).Returns(new FV.ValidationResult()); //mock validator says it found no errors

            //act
            var controllerResponse = _classUnderTest.GetAllTenancyTransactions(new GetAllTenancyTransactionsRequest());
            var result = controllerResponse as ObjectResult;

            //assert
            Assert.NotNull(controllerResponse);
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }

        [Test]
        public void given_an_invalid_request_when_GetAllTenancyTransactions_controller_method_is_called_then_it_returns_400_Bad_Request_result()
        {
            //arrange
            var expectedStatusCode = 400;
            var request = new GetAllTenancyTransactionsRequest();                                                                                       //an empty request will be invalid

            int errorCount = _faker.Random.Int(1, 10);                                                                                                  //simulate from 1 to 10 validation errors (triangulation).
            var validationErrorList = new List<ValidationFailure>();                                                                                    //this list will be used as constructor argument for 'ValidationResult'.
            for (int i = errorCount; i > 0; i--) { validationErrorList.Add(new ValidationFailure(_faker.Random.Word(), _faker.Random.Word())); }        //generate from 1 to 10 fake validation errors. Single line for-loop so that it wouldn't distract from what's key in this test.

            var fakeValidationResult = new FV.ValidationResult(validationErrorList);                                                                    //Need to create ValidationResult so that I could setup Validator mock to return it upon '.Validate()' call. Also this is the only place where it's possible to manipulate the validation result - You can only make the validation result invalid by inserting a list of validation errors as a parameter through a constructor. The boolean '.IsValid' comes from expression 'IsValid => Errors.Count == 0;', so it can't be set manually.
            _mockGetTenancyTransactionsValidator.Setup(v => v.Validate(It.IsAny<GetAllTenancyTransactionsRequest>())).Returns(fakeValidationResult);    //mock validator says that it has found errors

            //act
            var controllerResponse = _classUnderTest.GetAllTenancyTransactions(request);
            var result = controllerResponse as ObjectResult;

            //assert
            Assert.NotNull(controllerResponse);
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }

        [Test]
        public void given_an_invalid_request_when_GetAllTenancyTransactions_controller_method_is_called_then_returned_BadRequestObjectResult_contains_correctly_formatter_error_response()
        {
            //arrange
            var expectedStatusCode = 400;
            var request = new GetAllTenancyTransactionsRequest();                                                                                       //an empty request will be invalid

            int errorCount = _faker.Random.Int(1, 10);                                                                                                  //simulate from 1 to 10 validation errors (triangulation).
            var validationErrorList = new List<ValidationFailure>();                                                                                    //this list will be used as constructor argument for 'ValidationResult'.
            for (int i = errorCount; i > 0; i--) { validationErrorList.Add(new ValidationFailure(_faker.Random.Word(), _faker.Random.Word())); }        //generate from 1 to 10 fake validation errors. Single line for-loop so that it wouldn't distract from what's key in this test.

            var fakeValidationResult = new FV.ValidationResult(validationErrorList);                                                                    //Need to create ValidationResult so that I could setup Validator mock to return it upon '.Validate()' call. Also this is the only place where it's possible to manipulate the validation result - You can only make the validation result invalid by inserting a list of validation errors as a parameter through a constructor. The boolean '.IsValid' comes from expression 'IsValid => Errors.Count == 0;', so it can't be set manually.
            _mockGetTenancyTransactionsValidator.Setup(v => v.Validate(It.IsAny<GetAllTenancyTransactionsRequest>())).Returns(fakeValidationResult);    //mock validator says that it has found errors

            var expectedControllerResponse = new BadRequestObjectResult(validationErrorList);                                                           //build up expected controller response to check if the contents of the errors match - that's probably the easiest way to check that.

            //act
            var controllerResponse = _classUnderTest.GetAllTenancyTransactions(request);
            var result = controllerResponse as ObjectResult;
            var errorResponse = result.Value as ErrorResponse;

            //assert
            Assert.NotNull(result);

            Assert.IsInstanceOf<ErrorResponse>(result.Value);
            Assert.NotNull(result.Value);

            Assert.IsInstanceOf<string>(errorResponse.status);
            Assert.NotNull(errorResponse.status);
            Assert.AreEqual("fail", errorResponse.status);

            Assert.IsInstanceOf<List<string>>(errorResponse.errors);
            Assert.NotNull(errorResponse.errors);
            Assert.AreEqual(errorCount, errorResponse.errors.Count);
        }

        #endregion
    }
}
