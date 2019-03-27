using System;

namespace transactions_api.V1.Domain
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public int FinancialYear { get; set; }
        public Decimal PeriodNumber { get; set; }
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
                       string.Equals(Description,transaction.Description) &&
                       FinancialYear == transaction.FinancialYear &&
                       PeriodNumber == transaction.PeriodNumber &&
                       string.Equals(Comments, transaction.Comments);
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
                hashCode = (hashCode * 397) ^ (Comments != null ? Comments.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ PeriodNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ FinancialYear.GetHashCode();
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
 }
