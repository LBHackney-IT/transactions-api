
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

        #region GetTransactions (in general)

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

        #endregion

        #region GetTenancyTransactions

        //---------------------------------- Ported Code from NCC API (start) --------------------------------//

        public List<TenancyTransactions> GetAllTenancyTransactions(string tenancyAgreementRef, string startdate, string endDate)
        {
            SqlConnection uhtconn = new SqlConnection(_uhliveTransconnstring);
            uhtconn.Open();

            string fstartDate = Utils.FormatDate(startdate);
            string fendDate = (!string.IsNullOrEmpty(endDate)) ? Utils.FormatDate(endDate) : DateTime.Now.ToString("yyyy-MM-dd");
            string query =
                @" 
                    SELECT transno, 
                           rtrans.real_value AS Amount, 
                           rtrans.post_date  AS date, 
                           rtrans.trans_type AS Type, 
                           CASE 
                             WHEN rtrans.trans_type = 'DSB' THEN RTRIM(debtype.deb_desc) 
                             ELSE RTRIM(rectype.rec_desc) 
                           END               AS Description 
                    FROM   rtrans 
                           LEFT JOIN rectype 
                                  ON rtrans.trans_type = rectype.rec_code 
                           LEFT JOIN debtype 
                                  ON rtrans.trans_type = debtype.deb_code 
                    WHERE  tag_ref <> '' 
                           AND tag_ref <> 'ZZZZZZ' 
                           AND post_date BETWEEN @fstartDate AND @fendDate 
                           AND tag_ref = @tenancyAgreementRef 
                           AND ( trans_type IN (SELECT rec_code 
                                                FROM   rectype 
                                                WHERE  rec_group <= 8 
                                                        OR rec_code = 'RIT') 
                                  OR trans_type = 'DSB' ) 
                    UNION ALL 
                    SELECT 999999999999999999     AS transno, 
                           SUM(rtrans.real_value) AS Amount, 
                           post_date              AS Date, 
                           'RNT'                  AS Type, 
                           'Total Charge'         AS Description 
                    FROM   rtrans 
                    WHERE  tag_ref <> '' 
                           AND tag_ref <> 'ZZZZZZ' 
                           AND tag_ref = @tenancyAgreementRef 
                           AND post_date BETWEEN @fstartDate AND @fendDate 
                           AND rtrans.trans_type LIKE 'D%' 
                           AND rtrans.trans_type <> 'DSB' 
                           AND post_date = post_date 
                    GROUP  BY tag_ref, 
                              post_date, 
                              prop_ref, 
                              house_ref 
                    ORDER  BY post_date DESC, 
                              transno ASC 
                ";
            var results = uhtconn.Query<TenancyTransactions>(query, new { tenancyAgreementRef, fstartDate, fendDate }).ToList();
            uhtconn.Close();
            return results;
        }

        public TenancyAgreementDetials GetTenancyAgreementDetails(string tenancyAgreementRef)
        {
            SqlConnection uhtconn = new SqlConnection(_uhliveTransconnstring);
            uhtconn.Open();

            var result = uhtconn.QueryFirstOrDefault<TenancyAgreementDetials>(
                @"
                    SELECT cur_bal                           AS CurrentBalance, 
                           ( cur_bal *- 1 )                  AS DisplayBalance, 
                           ( rent + service + other_charge ) AS Rent, 
                           cot                               AS StartDate, 
                           RTRIM(house_ref)                  AS HousingReferenceNumber, 
                           RTRIM(prop_ref)                   AS PropertyReferenceNumber, 
                           RTRIM(u_saff_rentacc)             AS PaymentReferenceNumber, 
                           terminated                        AS IsAgreementTerminated, 
                           tenure                            AS TenureType 
                    FROM   tenagree 
                    WHERE  tag_ref = @tenancyAgreementRef 
                ",
                new { tenancyAgreementRef }
            );

            uhtconn.Close();

            return result;

        }

        public List<TenancyTransactionStatements> GetAllTenancyTransactionStatements(string tenancyAgreementId, string startdate, string endDate)
        {
            TenancyAgreementDetials tenantDet = GetTenancyAgreementDetails(tenancyAgreementId);
            List<TenancyTransactions> lstTransactions = GetAllTenancyTransactions(tenancyAgreementId, startdate, endDate);
            List<TenancyTransactionStatements> lstTransactionsState = new List<TenancyTransactionStatements>();
            float RecordBalance = 0;
            RecordBalance = float.Parse(tenantDet.CurrentBalance);

            foreach (TenancyTransactions trans in lstTransactions)
            {
                TenancyTransactionStatements statement = new TenancyTransactionStatements();
                var DebitValue = "";
                var CreditValue = "";
                float fDebitValue = 0F;
                float fCreditValue = 0F;
                var realvalue = trans.Amount;
                string DisplayRecordBalance = (-RecordBalance).ToString("c2");

                if (realvalue.IndexOf("-") != -1)
                {
                    DebitValue = realvalue;
                    fDebitValue = float.Parse(DebitValue);
                    RecordBalance = (RecordBalance - fDebitValue);
                    DebitValue = (-fDebitValue).ToString("c2");
                }
                else
                {
                    CreditValue = realvalue;
                    fCreditValue = float.Parse(CreditValue);
                    RecordBalance = (RecordBalance - fCreditValue);
                    CreditValue = (-fCreditValue).ToString("c2");
                }
                statement.Date = trans.Date;
                statement.Description = trans.Description;
                statement.In = DebitValue;
                statement.Out = CreditValue;
                statement.Balance = DisplayRecordBalance;
                lstTransactionsState.Add(statement);
            }

            return lstTransactionsState;

        }

        //---------------------------------- Ported Code from NCC API (end) --------------------------------//

        #endregion
    }
}
