
using System;
using System.Collections.Generic;
using System.Linq;
using transactions_api.V1.Domain;
using transactions_api.V1.Factory;
using UnitTests.V1.Infrastructure;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.V1.Gateways
{
    public class TransactionsGateway : ITransactionsGateway
    {
        private readonly IUHContext _uhcontext;
        private readonly TransactionFactory _transactionFactory;

        public TransactionsGateway(IUHContext uhcontext)
        {
            _transactionFactory = new TransactionFactory();
            _uhcontext = uhcontext;
        }

        public List<Transaction> GetTransactionsByTagRef(string tagRef)
        {
            var result = (from rtrans in _uhcontext.UTransactions
                          join debtype in _uhcontext.DebType
                              on rtrans.Code equals debtype.deb_code into dc
                          from debcode in dc.DefaultIfEmpty()
                          join rectype in _uhcontext.RecType
                              on rtrans.Code equals rectype.rec_code into rc
                          from reccode in rc.DefaultIfEmpty()
                          where rtrans.TagRef == tagRef
                          orderby rtrans.Date ascending
                          select new Transaction()
                          {
                              Date = rtrans.Date,
                              Code = rtrans.Code,
                              Description = rtrans.Code.StartsWith("D", StringComparison.CurrentCultureIgnoreCase)
                                  ? debcode.DebDescription
                                  : reccode.RecDescription,
                              Amount = rtrans.Amount,
                              Comments = rtrans.Comments,
                              FinancialYear = rtrans.FinancialYear,
                              PeriodNumber = rtrans.PeriodNumber
                          }).ToList();

            return result;
        }
    }
}
