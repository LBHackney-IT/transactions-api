using System;
using System.ComponentModel.DataAnnotations;
using transactions_api.V1.Validation;

namespace transactions_api.V1.Boundary
{
    public class ListTransactionsRequest
    {
        [Required] public string TagRef { get; set; }


        [CompareDates]
        [DateRange(ErrorMessage = "Date must be before or equal to current date")]
        public DateTime fromDate { get; set; }


        [DateRange(ErrorMessage = "Date must be before or equal to current date")]
        public DateTime toDate { get; set; }
    }
}
