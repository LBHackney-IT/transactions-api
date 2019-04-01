

using System;
using System.ComponentModel.DataAnnotations;

namespace transactions_api.V1.Boundary
{
    public class ListTransactionsRequest
    {
        [Required] public string TagRef { get; set; }
        [Required] public DateTime fromDate { get; set; }
        [Required] public DateTime toDate { get; set; }
    }
}
