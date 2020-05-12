using Microsoft.AspNetCore.Mvc;

namespace transactions_api.V1.Boundary
{
    public class GetPostcodeRequest
    {
        [FromRoute(Name = "payment_ref")] public string PaymentRef { get; set; }
    }
}
