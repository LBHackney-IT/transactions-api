
using System;
using System.Collections.Generic;
using System.Linq;
using transactions_api.V1.Domain;
using transactions_api.V1.Factory;
using UnitTests.V1.Infrastructure;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Text.RegularExpressions;

namespace UnitTests.V1.Gateways
{
    public class TransactionsGateway : ITransactionsGateway
    {
        private readonly IUHContext _uhcontext;
        private readonly TransactionFactory _transactionFactory;
        private readonly string _uhTransconnstring = Environment.GetEnvironmentVariable("UH_URL");

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

        public List<TempTenancyTransaction> GetAllTenancyTransactions(string tenancyAgreementRef)
        {
            SqlConnection uhtconn = new SqlConnection(_uhTransconnstring);
            uhtconn.Open();

            string query =                                                                                      // not sure how to limit 2 different queries to 5 results (UNION). OFFSET won't work.
                @" 
                    SELECT transno, 
                           rtrans.real_value AS Amount, 
                           rtrans.post_date  AS Date, 
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
            var results = uhtconn.Query<TempTenancyTransaction>(query, new { @tenancyAgreementRef = new DbString { Value = tenancyAgreementRef, IsFixedLength = true, IsAnsi = true, Length = 11 } }).ToList();  //<--------Can do .Take(5).ToList(); also commandTimeout: 0 is a hack to get around timeout that comes out of nowhere - query runs fine on SQL management studio
            uhtconn.Close();
            return results;
        }

        public TenancyAgreementDetails GetTenancyAgreementDetails(string paymentReferenceNumber, string postcode)
        {
            SqlConnection uhtconn = new SqlConnection(_uhTransconnstring);
            uhtconn.Open();

            postcode = Regex.Replace(postcode, @"\s+", String.Empty);

            var result = uhtconn.QueryFirstOrDefault<TenancyAgreementDetails>(
                @"
					SELECT TNG.cur_bal                                   AS CurrentBalance,
						   ( TNG.cur_bal *- 1 )                          AS DisplayBalance,
						   ( TNG.rent + TNG.service + TNG.other_charge ) AS Rent,
						   TNG.cot                                       AS StartDate,
						   Rtrim(TNG.house_ref)                          AS HousingReferenceNumber,
						   Rtrim(PRP.prop_ref)                           AS PropertyReferenceNumber,
						   Rtrim(TNG.tag_ref)                            AS TenancyAgreementReference,
						   Rtrim(TNG.u_saff_rentacc)                     AS PaymentReferenceNumber,
						   TNG.terminated                                AS IsAgreementTerminated,
						   TNG.tenure                                    AS TenureType
					FROM   tenagree TNG
						   INNER JOIN property PRP
								   ON TNG.prop_ref = PRP.prop_ref
					WHERE  TNG.u_saff_rentacc = @paymentReferenceNumber
						   AND REPLACE(PRP.post_code, ' ', '') = @postcode
                ",
                new { paymentReferenceNumber, postcode }
            );

            uhtconn.Close();

            return result;

        }

        public List<TenancyTransaction> GetAllTenancyTransactionStatements(string tenancyAgreementId, TenancyAgreementDetails tenantDet)
        {
            List<TempTenancyTransaction> lstTransactions = GetAllTenancyTransactions(tenancyAgreementId);
            List<TenancyTransaction> lstTransactionsState = new List<TenancyTransaction>();
            float RecordBalance = 0;
            RecordBalance = float.Parse(tenantDet.CurrentBalance);

            foreach (TempTenancyTransaction trans in lstTransactions)
            {
                TenancyTransaction statement = new TenancyTransaction();
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
