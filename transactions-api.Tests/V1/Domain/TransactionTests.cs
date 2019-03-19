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
        public void TransactionsHaveATagRef()
        {
            Transaction transaction = new Transaction();
            Assert.IsNull(transaction.TagRef);
        }

        [Test]
        public void TransactionsHaveAGrossValue()
        {
            Transaction transaction = new Transaction();
            Assert.Zero(transaction.GrossValue);
        }


        [Test]
        public void TransactionsHaveANetValue()
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
                TagRef = transactionA.TagRef,
                GrossValue = transactionA.GrossValue,
                NetValue = transactionA.NetValue
            };

            Assert.True(transactionA.Equals(transactionB));
            Assert.AreEqual(transactionA.GetHashCode(),transactionB.GetHashCode());

            Assert.AreNotSame(transactionA, transactionB);
            Assert.AreEqual(transactionA, transactionB);
        }
    }
}
