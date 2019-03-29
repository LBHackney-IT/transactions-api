using System.Linq;
using Bogus;
using NUnit.Framework;
using transactions_api.V1.Infrastructure;
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
            var responce = _classUnderTest.GetTransactionsByTagRef("random");
            Assert.AreEqual(0, responce.Count);
            Assert.AreEqual(null, responce.FirstOrDefault());
        }

        [Test]
      
        public void GetTransactionsByPropertyRef_ReturnsCorrectResponse()
        {
            Transaction transaction = TransactionHelper.CreateTransaction();

            UhTransaction dbTrans = UhTransactionHelper.CreateUhTransactionFrom(transaction);

            UhRecType recType = new UhRecType()
            {
                RecDescription = _faker.Random.Hash(10),
                rec_code = transaction.Code,
                rec_dd = _faker.Random.Bool(),
                rec_hb = _faker.Random.Bool(),
                Id = _faker.Random.Int()
            };

            _uhContext.UTransactions.Add(dbTrans);
            _uhContext.RecType.Add(recType);
            _uhContext.SaveChanges();

            var response = _classUnderTest.GetTransactionsByTagRef(dbTrans.TagRef).FirstOrDefault();

            Assert.AreEqual(transaction.Amount, response.Amount);
            Assert.AreEqual(transaction.Code, response.Code);
            Assert.That(transaction.Date, Is.EqualTo(response.Date).Within(1).Hours);
            Assert.AreEqual(transaction.Comments,response.Comments);
            Assert.AreEqual(transaction.PeriodNumber,response.PeriodNumber);
            Assert.AreEqual(transaction.FinancialYear,response.FinancialYear);
        }
    }
}
