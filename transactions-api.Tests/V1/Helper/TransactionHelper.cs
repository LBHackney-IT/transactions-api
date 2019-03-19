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
                Code = faker.Random.String(length: 3),
                TagRef = faker.Random.String(length: 5),
                GrossValue = faker.Finance.Amount(),
                NetValue = faker.Finance.Amount()
            };
            return transaction;
        }
    }
}
