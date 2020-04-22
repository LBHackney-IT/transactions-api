using Bogus;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Helper
{
    public static class TransactionHelper
    {
        private static Faker _faker = new Faker("en_GB");

        public static Transaction CreateTransaction()
        {
            var transaction = new Transaction
            {
                Date = _faker.Date.Past(),
                Code = _faker.Random.Hash(3),
                Description = _faker.Random.Hash(15),
                Amount = _faker.Finance.Amount(),
                Comments = _faker.Random.Hash(15),
                FinancialYear = _faker.Date.Past().Year,
                PeriodNumber = _faker.Random.Int(0,99),
                RunningBalance = _faker.Finance.Amount()
            };
            return transaction;
        }

        public static GetAllTenancyTransactionsRequest CreateGetAllTenancyTransactionsRequestObject()
        {
            return new GetAllTenancyTransactionsRequest()
            {
                PaymentRef = _faker.Random.Hash(),
                PostCode = _faker.Address.ZipCode()
            };
        }
    }
}
