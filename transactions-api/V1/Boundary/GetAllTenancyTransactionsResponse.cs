using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Domain;

namespace transactions_api.V1.Boundary
{
    public class GetAllTenancyTransactionsResponse
    {
        public DateTime GeneratedAt { get; set; }
        public GetAllTenancyTransactionsRequest Request { get; set; }
        public List<TenancyTransaction> Transactions { get; set; }
        public string CurrentBalance { get; set; }
    }
}
