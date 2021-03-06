using Bogus;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Helper
{
    public class TransactionHelper
    {
        public static Transaction CreateTransaction()
        {
            var faker = new Faker();
            var transaction = new Transaction
            {
                Date = faker.Date.Past(),
                Code = faker.Random.Hash(3),
                Description = faker.Random.Hash(15),
                Amount = faker.Finance.Amount(),
                Comments = faker.Random.Hash(15),
                FinancialYear = faker.Date.Past().Year,
                PeriodNumber = faker.Random.Int(0,99),
                RunningBalance = faker.Finance.Amount()
            };
            return transaction;
        }
    }
}
