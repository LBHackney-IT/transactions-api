using System;

namespace transactions_api.V1.Domain
{
    public class Transaction
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string TagRef { get; set; }
        public decimal GrossValue { get; set; }
        public decimal NetValue { get; set; }

        public override bool Equals(object obj)
        {
            Transaction transaction = obj as Transaction;
            if (transaction != null)
            {
                return string.Equals(Code, transaction.Code) &&
                       Date.Equals(transaction.Date) &&
                       string.Equals(a: TagRef, b: transaction.TagRef) &&
                       GrossValue == transaction.GrossValue &&
                       NetValue == transaction.NetValue;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GrossValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                return hashCode;
            }
        }
    }
 }
