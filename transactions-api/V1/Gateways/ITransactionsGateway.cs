using System.Collections.Generic;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Gateways
{
    public interface ITransactionsGateway
    {
        List<Transaction> GetTransactionsByTagRef(string propertyRef);
        List<TenancyTransaction> GetAllTenancyTransactionStatements(string tenancyAgreementId, TenancyAgreementDetails tenantDet); // I know it could do away with tenanDet only, but I feel like making tenancyAgreementId an explicit requirement right now..
        TenancyAgreementDetails GetTenancyAgreementDetails(string paymentReferenceNumber, string postcode);
        List<TempTenancyTransaction> GetAllTenancyTransactions(string tenancyAgreementRef);
    }
}
