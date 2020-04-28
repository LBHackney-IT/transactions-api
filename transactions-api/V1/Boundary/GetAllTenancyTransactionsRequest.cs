using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Boundary
{
    public class GetAllTenancyTransactionsRequest
    {
        [FromRoute(Name = "payment_ref")]   public string PaymentRef { get; set; }
        [FromRoute(Name = "post_code")]     public string PostCode { get; set; }
    }
}
