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

            Assert.AreEqual(uhTransaction.Amount,transaction.Amount);
            Assert.AreEqual(uhTransaction.Code,transaction.Code);
            Assert.AreEqual(uhTransaction.Date,transaction.Date);
            Assert.AreEqual(uhTransaction.NetValue, transaction.NetValue);
            Assert.AreEqual(uhTransaction.VatValue, transaction.VatValue);
        }
    }
}
