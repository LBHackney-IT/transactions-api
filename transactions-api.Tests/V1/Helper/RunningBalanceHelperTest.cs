using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using NUnit.Framework;
using transactions_api.V1.Domain;
using transactions_api.V1.Helpers;
using UnitTests.V1.Helper;

namespace transactions_api.Tests.V1.Helper
{
    [TestFixture]
    public class RunningBalanceHelperTest
    { 
        [Test]
        public void CalculateRunningBalance_ShouldReturnCorrectCalculations()
        {
            Transaction transactionA = TransactionHelper.CreateTransaction();
            Transaction transactionB = TransactionHelper.CreateTransaction();

            List<Transaction> listOfTransactions = new List<Transaction>(){transactionA,transactionB};

            transactionA.RunningBalance = transactionA.Amount;
            transactionB.RunningBalance = transactionB.Amount + transactionA.RunningBalance;

            var expectedResult = RunningBalanceHelper.CalculateRunningBalance(listOfTransactions);

            Assert.AreEqual(transactionA.RunningBalance, expectedResult[0].RunningBalance);
            Assert.AreEqual(transactionB.RunningBalance, expectedResult[1].RunningBalance);
        }

    }
}
