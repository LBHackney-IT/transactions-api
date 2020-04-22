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
using UnitTests.V1.Helper;

namespace UnitTests.V1.Controllers
{

    [TestFixture]
    public class TransactionControllerTests
    {
        private TransactionsController _classUnderTest;

        private Mock<IListTransactions> _mockListTransacionsUsecase;

        private Faker faker = new Faker();

        [SetUp]
        public void SetUp()
        {
            _mockListTransacionsUsecase = new Mock<IListTransactions>();

            ILogger<TransactionsController> nullLogger = NullLogger<TransactionsController>.Instance;
            _classUnderTest = new TransactionsController(_mockListTransacionsUsecase.Object, nullLogger);
        }

        #region Transactions in General

        [Test]
        public void ReturnsCorrectRandomResponseWithStatus()
        {
            var transaction = TransactionHelper.CreateTransaction();
            var request = ListTransactionsRequest();
            var datetime = faker.Date.Past();

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
            var faker = new Faker();
            var listTransactionsRequest = new ListTransactionsRequest
            {
                TagRef = faker.Random.Hash(9),
                fromDate = faker.Date.Past(),
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
        public void given_a_valid_input_when_GetAllTenancyTransactions_controller_method_is_called_then_it_returns_200_Ok_result()
        {
            //arrange
            var expectedStatusCode = 200;
            //TODO: Add validator mock setup to return no errors, when one is created!

            //act
            var controllerResponse = _classUnderTest.GetAllTenancyTransactions(new GetAllTenancyTransactionsRequest());
            var result = controllerResponse as ObjectResult;

            //assert
            Assert.NotNull(controllerResponse);
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }

        #endregion
    }
}
