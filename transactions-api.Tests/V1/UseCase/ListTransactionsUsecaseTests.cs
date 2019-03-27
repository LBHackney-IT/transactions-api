using System.Collections.Generic;
using System.Linq;
using Bogus;
using NUnit.Framework;
using transactions_api.UseCase;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;
using Moq;
using transactions_api.V1.Helpers;
using UnitTests.V1.Gateways;
using UnitTests.V1.Helper;

namespace UnitTests.V1.UseCase
{
    [TestFixture]
    public class ListTransactionsUsecaseTests
    {
        private ListTransactionsUsecase _classUnderTest;
        private Mock<ITransactionsGateway> _transactionsGateway;
        private Faker _faker;

        [SetUp]
        public void Setup()
        {
            _transactionsGateway = new Mock<ITransactionsGateway>();
            _classUnderTest = new ListTransactionsUsecase(_transactionsGateway.Object);
            _faker = new Faker();
        }

        [Test]
        public void ListOfTransactionsImplementsBoundaryInterface()
        {
            Assert.True(_classUnderTest is IListTransactions);
        }

        [Test]
        public void CanGetListOfTransactionsByPropertyReference()
        {
            var propertyRef = _faker.Random.Hash();
            var request = new ListTransactionsRequest {PropertyRef = propertyRef};

            List<Transaction> response = new List<Transaction> {new Transaction()};

            _transactionsGateway.Setup(foo => foo.GetTransactionsByPropertyRef(propertyRef)).Returns(response);

            var results = _classUnderTest.Execute(request);

            Assert.NotNull(results);
            Assert.IsInstanceOf<ListTransactionsResponse>(results);
            Assert.IsInstanceOf<Transaction>(results.Transactions.First());

            Assert.IsInstanceOf<ListTransactionsRequest>(results.Request);
            Assert.AreEqual(propertyRef, results.Request.PropertyRef);
        }

        [Test]
        public void ExecuteCallsTransactionGateway()
        {
            var propertyRef = _faker.Random.Hash();

            var request = new ListTransactionsRequest {PropertyRef = propertyRef};

            _classUnderTest.Execute(request);

            _transactionsGateway.Verify(gateway => gateway.GetTransactionsByPropertyRef(propertyRef));
        }

        [Test]
        public void ExecuteReturnsResponceUsingGatewayResults()
        {
            var propertyRef = _faker.Random.Hash();

            var request = new ListTransactionsRequest {PropertyRef = propertyRef};

            List<Transaction> response = new List<Transaction>{ new Transaction(), new Transaction()};

            _transactionsGateway.Setup(foo => foo.GetTransactionsByPropertyRef(propertyRef)).Returns(response);

            var result = _classUnderTest.Execute(request);

            Assert.AreEqual(response, result.Transactions);

        }

        [TestCase("abc")]
        [TestCase("bcd")]
        [TestCase("asdasdas")]
        public void ExecuteReturnsOBjectWithRunningBalancePopulated(string propRef)
        {
            var propertyRef = propRef;
            var request = new ListTransactionsRequest(){PropertyRef = propertyRef};

            Transaction transactionA = TransactionHelper.CreateTransaction();
            Transaction transactionB = TransactionHelper.CreateTransaction();

            List<Transaction> listOfTransactions = new List<Transaction>() { transactionA, transactionB };

            _transactionsGateway.Setup(x => x.GetTransactionsByPropertyRef(propertyRef)).Returns(listOfTransactions);

            listOfTransactions = listOfTransactions?.CalculateRunningBalance();

            var expectedResult = _classUnderTest.Execute(request);

            Assert.AreEqual(expectedResult.Transactions,listOfTransactions);
        }

        [Test]
        public void ExecuteReturnsOBjectWithRunningBalanceUnPopulated()
        {
            var propertyRef = _faker.Random.Hash(9);
            var request = new ListTransactionsRequest() { PropertyRef = propertyRef };

            _transactionsGateway.Setup(x => x.GetTransactionsByPropertyRef(propertyRef)).Returns(()=>null);

            var expectedResult = _classUnderTest.Execute(request);

            Assert.IsNull(expectedResult.Transactions);
        }

    }
}
