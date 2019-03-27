using System;
using Bogus;
using NUnit.Framework;
using transactions_api.V1.Domain;
using transactions_api.V1.Factory;
using UnitTests.V1.Helper;

namespace UnitTests.V1.Domain
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void TransactionsHaveRunningBalance()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.RunningBalance);
        }

        [Test]
        public void TransactionsHaveACode()
        {
            Transaction transaction = new Transaction();
            Assert.IsNull(transaction.Code);
        }

        [Test]
        public void TransactionsHaveADateTime()
        {
            Transaction transaction = new Transaction();
            DateTime date = new DateTime(2019, 02, 21);
            transaction.Date = date;
            Assert.AreEqual(date, transaction.Date);
        }
        [Test]
        public void TransactionsHaveComments()
        {
            Transaction transaction = new Transaction();
            Assert.IsNull(transaction.Comments);
        }

        [Test]
        public void TransactionsHaveADescription()
        {
            Transaction transaction = new Transaction();
            Assert.IsNull(transaction.Description);
        }

        [Test]
        public void TransactionsHaveAmount()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.Amount);
        }

        [Test]
        public void TransactionsHaveFinancialYearYear()
        {
            Transaction transaction = new Transaction();
            int year = new DateTime(2019, 02, 21).Year;
            transaction.FinancialYear = year;
            Assert.AreEqual(year, transaction.FinancialYear);
        }

        [Test]
        public void TransactionsHavePeriodNumber()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.PeriodNumber);
        }

        [Test]
        public void TransactionsCanBeCompared()
        {
            Transaction transactionA = TransactionHelper.CreateTransaction();

            Transaction transactionB = new Transaction
            {
                Date = transactionA.Date,
                Code = transactionA.Code,
                Description = transactionA.Description,
                Amount = transactionA.Amount,
                Comments = transactionA.Comments,
                PeriodNumber = transactionA.PeriodNumber,
                FinancialYear = transactionA.FinancialYear,
                RunningBalance = transactionA.RunningBalance
            };

            Assert.True(transactionA.Equals(transactionB));
            Assert.AreEqual(transactionA.GetHashCode(),transactionB.GetHashCode());
            Assert.AreEqual(transactionA.Comments, transactionB.Comments);
            Assert.AreEqual(transactionA.FinancialYear,transactionB.FinancialYear);
            Assert.AreEqual(transactionA.PeriodNumber,transactionB.PeriodNumber);
            Assert.AreEqual(transactionA.Date,transactionB.Date);
            Assert.AreNotSame(transactionA, transactionB);
            Assert.AreEqual(transactionA, transactionB);
        }
    }
}
