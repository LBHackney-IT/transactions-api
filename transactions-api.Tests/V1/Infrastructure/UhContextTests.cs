using System.Linq;
using NUnit.Framework;
using transactions_api.V1.Domain;
using UnitTests.V1.Helper;

namespace UnitTests.V1.Infrastructure
{
    [TestFixture]
    public class UhContextTest : DbTest
    {
        [Test]
        public void TestDbIsEmpty()
        {
            //if this fails you are not using an empty test db
            var result = _uhContext.UTransactions.Count();
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CanGetAUhTransaction()
        {
            UhTransaction uhTransaction = UhTransactionHelper.CreateUhTransaction();

            _uhContext.Add(uhTransaction);
            _uhContext.SaveChanges();

            var result = _uhContext.UTransactions.ToList().FirstOrDefault();

            Assert.AreEqual(uhTransaction, result);
            Assert.AreEqual(uhTransaction, result);
        }
    }
}
