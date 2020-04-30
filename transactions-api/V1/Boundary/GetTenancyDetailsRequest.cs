using Microsoft.AspNetCore.Mvc;

namespace transactions_api.V1.Boundary
{
    public class GetTenancyDetailsRequest
    {
        [FromRoute(Name = "payment_ref")] public string PaymentRef { get; set; }
        [FromRoute(Name = "post_code")] public string PostCode { get; set; }
    }
}
