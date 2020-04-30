using System.Collections.Generic;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Gateways
{
    public interface ITransactionsGateway
    {
        List<Transaction> GetTransactionsByTagRef(string propertyRef);
        List<TenancyTransaction> GetAllTenancyTransactionStatements(string paymentReferenceNumber, string postcode);
        TenancyAgreementDetails GetTenancyAgreementDetails(string paymentReferenceNumber, string postcode);
        List<TempTenancyTransaction> GetAllTenancyTransactions(string tenancyAgreementRef);
    }
}
