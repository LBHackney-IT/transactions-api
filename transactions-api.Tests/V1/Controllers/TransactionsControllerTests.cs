using System;
using System.Collections.Generic;
using Bogus;
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
                        {"propertyRef", request.PropertyRef}
                    }
                },
                { "generatedAt", datetime},
                { "transactions", new [] { new Dictionary<string, object>
                        {
                            {"code", transaction.Code},
                            {"date", transaction.Date},
                            {"tagRef", transaction.TagRef},
                            {"grossValue", transaction.GrossValue},
                            {"netValue", transaction.NetValue}

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
                PropertyRef = faker.Random.Hash()
            };
            return listTransactionsRequest;
        }

        [Test]
        public void ReturnsCorrectResponseWithStatus()
        {
            var transaction = new Transaction
            {
                Code = "Field",
                Date = new DateTime(2019, 02, 22, 09, 52, 23, 22),
                TagRef = "0012/01",
                GrossValue = 100.00m,
                NetValue = 120.00m
            };
            var request = new ListTransactionsRequest
            {
                PropertyRef = "testString"
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
    ""propertyRef"": ""testString""
  },
  ""generatedAt"": ""2019-02-22T09:52:23.023Z"",
  ""transactions"": [
    {
      ""code"": ""Field"",
      ""date"": ""2019-02-22T09:52:23.022Z"",
      ""tagRef"": ""0012/01"",
      ""grossValue"": 100.00,
      ""netValue"": 120.00
    }
  ]
}";
            return json;
        }
    }
}
