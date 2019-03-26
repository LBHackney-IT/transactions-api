using Microsoft.EntityFrameworkCore;
using transactionsapi.V1.Infrastructure;
using transactions_api.V1.Domain;
using transactions_api.V1.Infrastructure;

namespace UnitTests.V1.Infrastructure
{
    public interface IUHContext
    {
        DbSet<UhTransaction> UTransactions { get; set; }
        DbSet<UhDebType> DebType { get; set; }
        DbSet<UhRecType> RecType { get; set; }
    }
}
