using Microsoft.EntityFrameworkCore;
using transactions_api.V1.Domain;

namespace UnitTests.V1.Infrastructure
{
    public interface IUHContext
    {
        DbSet<UhTransaction> UTransactions { get; set; }
    }
}