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
        public void TransactionsHaveVATValue()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.VatValue);
        }


        [Test]
        public void TransactionsHaveNetValue()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.NetValue);
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
                VatValue = transactionA.VatValue,
                NetValue = transactionA.NetValue,
                RunningBalance = transactionA.RunningBalance
            };

            Assert.True(transactionA.Equals(transactionB));
            Assert.AreEqual(transactionA.GetHashCode(),transactionB.GetHashCode());

            Assert.AreNotSame(transactionA, transactionB);
            Assert.AreEqual(transactionA, transactionB);
        }
    }
}
