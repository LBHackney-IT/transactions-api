using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using transactions_api.V1.Domain;

namespace transactions_api.V1.Helpers
{
    public static class RunningBalanceHelper
    {
        public static List<Transaction> CalculateRunningBalance(this List<Transaction> listOfTransactions)
        {
            //running balance starts from 0
            decimal runningBalance = 0;
            foreach (var transaction in listOfTransactions)
            {
                runningBalance = transaction.Amount + runningBalance;
                transaction.RunningBalance = runningBalance;
            }
            return listOfTransactions;
        }
    }
}
