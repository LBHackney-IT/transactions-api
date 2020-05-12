using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace transactions_api.V1.Boundary
{
    public class GetPostcodeRequest
    {
        [Required] public string PaymentRef { get; set; }
    }
}
