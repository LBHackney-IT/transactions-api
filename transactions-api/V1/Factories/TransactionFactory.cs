using transactions_api.V1.Domain;

namespace transactions_api.V1.Factory
{
    public class TransactionFactory : AbstractTransactionFactory
    {
        public override Transaction FromUhTransaction(UhTransaction transaction)
        {
            return new Transaction
            {
                GrossValue = transaction.full_value,
                Code = transaction.Code,
                Date = transaction.Date
            };
        }
    }
}
