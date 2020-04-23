using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transactions_api.V1.Domain                    //This is where the "porting from NCC" starts
{
    public class TenancyTransaction                     //'In' and 'Out' - don't think it makes sense to have it like this long term. I think it's Front-End's responsibility to distinguish between positive and negative amounts.
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public string In { get; set; }
        public string Out { get; set; }
        public string Balance { get; set; }
    }

    public class TempTenancyTransaction                 //Temporary class from NCC so that the data from sql query would get automapped. But I think this should be our return object with an addition of balance.
    {
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}