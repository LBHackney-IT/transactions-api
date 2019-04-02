using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using transactions_api.V1.Boundary;
using transactions_api.V1.Domain;

namespace transactions_api.V1.Helpers
{
    public static class TransactionsFilters
    {
        public static List<Transaction> FilterTransactions(this List<Transaction> listOfTransactions, ListTransactionsRequest transactionsRequest)
        {
            //if user does not pass toDate, make toDate today's date. 
            var toDate = transactionsRequest.toDate == DateTime.MinValue
                ? DateTime.Now : transactionsRequest.toDate;
            //if user hasn't passed fromDate, use default (DateTime minValue) 
            return listOfTransactions.FindAll(x => x.Date >= transactionsRequest.fromDate  && x.Date <= toDate);
        }
    }
}
