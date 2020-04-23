using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Domain
{
    public class TenancyTransaction
    {
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
