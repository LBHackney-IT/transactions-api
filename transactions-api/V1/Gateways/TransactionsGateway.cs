
using System.Collections.Generic;
using System.Linq;
using transactions_api.V1.Domain;
using transactions_api.V1.Factory;
using UnitTests.V1.Infrastructure;

namespace UnitTests.V1.Gateways
{
    public class TransactionsGateway : ITransactionsGateway
    {
        private readonly IUHContext _uhcontext;
        private readonly TransactionFactory _transactionFactory;

        public TransactionsGateway(IUHContext uhcontext)
        {
            _transactionFactory = new TransactionFactory();
            _uhcontext = uhcontext;
        }

        public List<Transaction> GetTransactionsByTagRef(string tagRef)
        {
            var result = _uhcontext.UTransactions.Where(t => t.TagRef == tagRef).ToList();
            return _transactionFactory.FromUhTransaction(result);
        }
    }
}
