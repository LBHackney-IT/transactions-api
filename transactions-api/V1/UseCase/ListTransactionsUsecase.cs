using System;
using transactions_api.V1.Boundary;
using transactions_api.V1.Helpers;
using UnitTests.V1.Gateways;

namespace transactions_api.UseCase
{
    public class ListTransactionsUsecase : IListTransactions
    {
        private readonly ITransactionsGateway _transactionsGateway;

        public ListTransactionsUsecase(ITransactionsGateway transactionsGateway)
        {
            _transactionsGateway = transactionsGateway;
        }

        public ListTransactionsResponse Execute(ListTransactionsRequest listTransactionsRequest)
        {
            var results = _transactionsGateway.GetTransactionsByTagRef(listTransactionsRequest.TagRef);

            results = results?.CalculateRunningBalance();
              
            results = results?.FilterTransactions(listTransactionsRequest);

            return new ListTransactionsResponse(results, listTransactionsRequest, DateTime.Now);
        }

        public GetAllTenancyTransactionsResponse ExecuteGetTenancyTransactions(GetAllTenancyTransactionsRequest request)
        {
            //GetTenancyAgreementDetails(pay_ref, postcode) --> tag_ref, cur_bal

            //GetAllTenancyTransactionStatements(tag_ref) --> get back a list...o TOP 5
            throw new NotImplementedException();
        }
    }
}
