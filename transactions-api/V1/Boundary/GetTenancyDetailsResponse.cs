using System;
using System.Collections.Generic;
using transactions_api.V1.Domain;

namespace transactions_api.V1.Boundary
{
    public class GetTenancyDetailsResponse
    {
        public DateTime GeneratedAt { get; set; }
        public GetTenancyDetailsRequest Request { get; set; }
        public TenancyAgreementDetails TenancyDetails { get; set; }
    }
}
