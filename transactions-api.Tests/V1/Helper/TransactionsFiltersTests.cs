using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using NUnit.Framework;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;
using transactions_api.V1.Helpers;
using UnitTests.V1.Helper;

namespace transactions_api.Tests.V1.Helper
{
    [TestFixture]
    public class TransactionsFiltersTests
    {

        private Faker _faker;
        [SetUp]
        public void SetUp()
        {
            _faker = new Faker();
        }

        [TestCase("20-03-2018" , "20-03-2019", "20-02-2018")]
        [TestCase("22-05-2018", "20-03-2019", "20-02-2018")]
        public void TransactionsFilterHelper_ShouldReturnFilteredTransactions(string fromDate, string toDate, string outOfRangeDate)
        {
            Transaction transactionA = TransactionHelper.CreateTransaction();
            //make transaction A has a date within the filter criteria
            transactionA.Date = DateTime.Parse(fromDate);
            Transaction transactionB = TransactionHelper.CreateTransaction();
            //transaction B has a date that is out of range so it is excluded from the results
            transactionB.Date = DateTime.Parse(outOfRangeDate);

            List<Transaction> listOfTransactions = new List<Transaction>(){transactionA, transactionB};

            List<Transaction> filteredTransactions = new List<Transaction>(){transactionA};

            ListTransactionsRequest request = new ListTransactionsRequest(){TagRef = _faker.Random.Hash(9), fromDate = DateTime.Parse(fromDate), toDate = DateTime.Parse(toDate)};

            var expectedResult = listOfTransactions.FilterTransactions(request);
            Assert.AreEqual(expectedResult, filteredTransactions);
        }
    }
}
