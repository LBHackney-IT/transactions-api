using System;

namespace transactions_api.V1.Domain
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public DateTime FinancialYear { get; set; }
        public Decimal PeriodNumber { get; set; }
        public int BatchId { get; set; }
        public string BatchCreatedBy { get; set; }
        public string BatchCreatedOn { get; set; }
        public DateTime BatchPostedOn { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }

        public Decimal Amount { get; set; }
        public Decimal RunningBalance { get; set; }
       
        public override bool Equals(object obj)
        {
            Transaction transaction = obj as Transaction;
            if (transaction != null)
            {
                return RunningBalance == transaction.RunningBalance &&
                       string.Equals(Code, transaction.Code) &&
                       Date.Equals(transaction.Date) &&
                       Amount == transaction.Amount &&
                       string.Equals(Description,transaction.Description);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RunningBalance.GetHashCode();
                hashCode = (hashCode * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                return hashCode;
            }
        }
    }
 }
