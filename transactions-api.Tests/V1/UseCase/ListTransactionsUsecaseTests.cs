using System.Collections.Generic;
using System.Linq;
using Bogus;
using NUnit.Framework;
using transactions_api.UseCase.V1;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;
using Moq;
using UnitTests.V1.Gateways;

namespace UnitTests.V1.UseCase
{
    [TestFixture]
    public class ListTransactionsUsecaseTests
    {
        private ListTransactionsUsecase _classUnderTest;
        private Mock<ITransactionsGateway> _transactionsGateway;

        [SetUp]
        public void Setup()
        {
            _transactionsGateway = new Mock<ITransactionsGateway>();
            _classUnderTest = new ListTransactionsUsecase(_transactionsGateway.Object);
        }

        [Test]
        public void ListOfTransactionsImplementsBoundaryInterface()
        {
            Assert.True(_classUnderTest is IListTransactions);
        }

        [Test]
        public void CanGetListOfTransactionsByTagReference()
        {
            var tagRef = new Faker().Random.Hash();
            var request = new ListTransactionsRequest {TagRef = tagRef };

            List<Transaction> response = new List<Transaction> {new Transaction()};

            _transactionsGateway.Setup(foo => foo.GetTransactionsByTagRef(tagRef)).Returns(response);

            var results = _classUnderTest.Execute(request);

            Assert.NotNull(results);
            Assert.IsInstanceOf<ListTransactionsResponse>(results);
            Assert.IsInstanceOf<Transaction>(results.Transactions.First());

            Assert.IsInstanceOf<ListTransactionsRequest>(results.Request);
            Assert.AreEqual(tagRef, results.Request.TagRef);
        }

        [Test]
        public void ExecuteCallsTransactionGateway()
        {
            var tagRef = new Faker().Random.Hash();

            var request = new ListTransactionsRequest {TagRef = tagRef };

            _classUnderTest.Execute(request);

            _transactionsGateway.Verify(gateway => gateway.GetTransactionsByTagRef(tagRef));
        }

        [Test]
        public void ExecuteReturnsResponceUsingGatewayResults()
        {
            var tagRef = new Faker().Random.Hash();

            var request = new ListTransactionsRequest {TagRef = tagRef };

            List<Transaction> response = new List<Transaction>{ new Transaction(), new Transaction()};

            _transactionsGateway.Setup(foo => foo.GetTransactionsByTagRef(tagRef)).Returns(response);


            var result = _classUnderTest.Execute(request);

            Assert.AreEqual(response, result.Transactions);

        }
    }
}
