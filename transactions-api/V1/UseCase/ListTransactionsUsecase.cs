using System;
using System.Collections.Generic;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;
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
            var transactions = _transactionsGateway.GetAllTenancyTransactionStatements(request.PaymentRef, request.PostCode) ?? new List<TenancyTransaction>();

            return new GetAllTenancyTransactionsResponse()
            {
                GeneratedAt = DateTime.Now,
                Request = request,
                Transactions = transactions
            };
        }

        public GetTenancyDetailsResponse ExecuteGetTenancyDetails(GetTenancyDetailsRequest request)
        {
            var tenancyDetails = _transactionsGateway.GetTenancyAgreementDetails(request.PaymentRef, request.PostCode);

            return new GetTenancyDetailsResponse()
            {
                GeneratedAt = DateTime.Now,
                Request = request,
                TenancyDetails = tenancyDetails
            };
        }
    }
}
