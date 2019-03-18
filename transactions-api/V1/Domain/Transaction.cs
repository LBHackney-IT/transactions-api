using System;

namespace transactions_api.V1.Domain
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public Decimal NetValue { get; set; }
        public Decimal VatValue { get; set; }
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
                       VatValue == transaction.VatValue &&
                       NetValue == transaction.NetValue &&
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
