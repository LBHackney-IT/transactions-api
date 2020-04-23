using System;
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

        #region GetTransactions (in general)

        [Test]
        public void CanGetListOfTransactionsByTagference()
        {
            var tagRef = _faker.Random.Hash();
            var request = new ListTransactionsRequest {TagRef = tagRef};

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
            var tagRef = _faker.Random.Hash();

            var request = new ListTransactionsRequest {TagRef = tagRef, fromDate = _faker.Date.Past(), toDate = _faker.Date.Past() };

            _classUnderTest.Execute(request);

            _transactionsGateway.Verify(gateway => gateway.GetTransactionsByTagRef(tagRef));
        }

        [Test]
        public void ExecuteReturnsResponceUsingGatewayResults()
        {
            var tagRef = _faker.Random.Hash();

            var request = new ListTransactionsRequest {TagRef = tagRef};

            List<Transaction> response = new List<Transaction>{ new Transaction(), new Transaction()};

            _transactionsGateway.Setup(foo => foo.GetTransactionsByTagRef(tagRef)).Returns(response);

            var result = _classUnderTest.Execute(request);

            Assert.AreEqual(response, result.Transactions);

        }

        [TestCase("abc","20-02-2018", "21-04-2019")]
        [TestCase("bcd", "20-06-2018", "11-06-2019")]
        [TestCase("asdasdas", "20-12-2018", "21-04-2019")]
        public void ExecuteReturnsOBjectWithRunningBalancePopulated(string tagReference,string fromDate, string toDate)
        {
            var tagRef = tagReference;

            Transaction transactionA = TransactionHelper.CreateTransaction();
            //make transaction A and B date be within filter criteria
            transactionA.Date = DateTime.Parse(fromDate);
            Transaction transactionB = TransactionHelper.CreateTransaction();
            transactionB.Date = DateTime.Parse(toDate);
            var request = new ListTransactionsRequest(){TagRef = tagRef, fromDate = DateTime.Parse(fromDate), toDate = DateTime.Parse(toDate) };

            List<Transaction> listOfTransactions = new List<Transaction>() { transactionA, transactionB };

            _transactionsGateway.Setup(x => x.GetTransactionsByTagRef(tagRef)).Returns(listOfTransactions);

            listOfTransactions = listOfTransactions?.CalculateRunningBalance();

            var expectedResult = _classUnderTest.Execute(request);

            Assert.AreEqual(expectedResult.Transactions,listOfTransactions);
        }

        [Test]
        public void ExecuteReturnsOBjectWithRunningBalanceUnPopulated()
        {
            var tagRef = _faker.Random.Hash(9);
            var request = new ListTransactionsRequest() { TagRef = tagRef, fromDate = _faker.Date.Past(), toDate = _faker.Date.Past()};

            _transactionsGateway.Setup(x => x.GetTransactionsByTagRef(tagRef)).Returns(()=>null);

            var expectedResult = _classUnderTest.Execute(request);

            Assert.IsNull(expectedResult.Transactions);
        }

        
        [TestCase("18-03-2018", "18-03-2019", "10-10-2008")]
        [TestCase("20-03-2017", "01-04-2019", "10-11-2012")]
        public void ExecuteReturnsAFilteredSetOfTransactions(string fromDate, string toDate, string outOfRangeDate)
        {
            var tagRef = _faker.Random.Hash(9);

            Transaction transactionA = TransactionHelper.CreateTransaction();
            transactionA.Date = DateTime.Parse(fromDate);
            Transaction transactionB = TransactionHelper.CreateTransaction();
            //transaction B has a date that is out of range so we can test if filtering occurs
            transactionB.Date = DateTime.Parse(outOfRangeDate);
            var request = new ListTransactionsRequest() { TagRef = tagRef,fromDate = DateTime.Parse(fromDate), toDate = DateTime.Parse(toDate)};

            List<Transaction> listOfTransactions = new List<Transaction>() { transactionA, transactionB };
            List<Transaction> listOfFilteredTransactions = new List<Transaction>() {transactionA};

            _transactionsGateway.Setup(x => x.GetTransactionsByTagRef(tagRef)).Returns(listOfTransactions);

            var expectedResult = _classUnderTest.Execute(request);

            Assert.AreEqual(expectedResult.Transactions, listOfFilteredTransactions);
        }

        #endregion

        #region GetTenancyTransactions

        // this is where TDD breaks down due to port being temp solution - will need to add tests in the future

        #endregion
    }
}
