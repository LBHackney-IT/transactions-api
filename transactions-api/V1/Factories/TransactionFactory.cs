using transactions_api.V1.Domain;

namespace transactions_api.V1.Factory
{
    public class TransactionFactory : AbstractTransactionFactory
    {
        public override Transaction FromUhTransaction(UhTransaction transaction)
        {
            return new Transaction
            {
                Date = transaction.Date,
                Code = transaction.Code,
                Amount = transaction.Amount
            };
        }
    }
}
