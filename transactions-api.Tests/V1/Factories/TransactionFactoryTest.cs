using NUnit.Framework;
using transactions_api.V1.Domain;
using transactions_api.V1.Factory;

namespace UnitTests.V1.Factories
{
    [TestFixture]
    public class TransactionFactoryTest
    {
        [Test]
        public void CanBeCreatedFromUhTransactions()
        {
            var uhTransaction = new UhTransaction();

            var transaction = new TransactionFactory().FromUhTransaction(uhTransaction);

            Assert.AreEqual(uhTransaction.Balance,transaction.RunningBalance);
            Assert.AreEqual(uhTransaction.Code,transaction.Code);
            Assert.AreEqual(uhTransaction.Date,transaction.Date);
        }
    }
}
