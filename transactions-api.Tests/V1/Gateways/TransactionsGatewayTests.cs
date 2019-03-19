using System.Linq;
using Bogus;
using NUnit.Framework;
using transactions_api.V1.Domain;
using UnitTests.V1.Helper;

namespace UnitTests.V1.Gateways
{
    [TestFixture]
    public class TransactionsGatewayTests : DbTest
    {
        private readonly Faker _faker = new Faker();
        private TransactionsGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new TransactionsGateway(_uhContext);
        }

        [Test]
        public void ListOfTransactionsImplementsBoundaryInterface()
        {
            Assert.NotNull(_classUnderTest is ITransactionsGateway);
        }

        [Test]
        public void GetTransactionsByPropertyRef_ReturnsEmptyArray()
        {
            var responce = _classUnderTest.GetTransactionsByPropertyRef("random");

            Assert.AreEqual(0, responce.Count);
            Assert.AreEqual(null, responce.FirstOrDefault());
        }

        [Test]
        public void GetTransactionsByPropertyRef_ReturnsCorrectResponse()
        {
            Transaction transaction = TransactionHelper.CreateTransaction();

            UhTransaction dbTrans = UhTransactionHelper.CreateUhTransactionFrom(transaction);

            _uhContext.UTransactions.Add(dbTrans);
            _uhContext.SaveChanges();

            var response = _classUnderTest.GetTransactionsByPropertyRef(dbTrans.PropRef).FirstOrDefault();
            
            Assert.AreEqual(transaction.GrossAmount, response.GrossAmount);
            Assert.AreEqual(transaction.Code, response.Code);
            Assert.AreEqual(transaction.Date.Date, response.Date.Date);
            Assert.AreEqual(transaction.NetValue, response.NetValue);
            Assert.AreEqual(transaction.VatValue, response.VatValue);
        }
    }
}
