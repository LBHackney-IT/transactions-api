namespace transactions_api.V1.Boundary
{
    public interface IListTransactions
    {
        ListTransactionsResponse Execute(ListTransactionsRequest propertyRefrence);
        GetAllTenancyTransactionsResponse ExecuteGetTenancyTransactions(GetAllTenancyTransactionsRequest request);
        GetTenancyDetailsResponse ExecuteGetTenancyDetails(GetTenancyDetailsRequest request);
        GetPostcodeResponse ExecuteGetPostcode(GetPostcodeRequest request);
    }
}
