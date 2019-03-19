using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transactions_api.V1.Domain
{
    [Table("rtrans")]
    public class UhTransaction
    {
        [Column("prop_ref")] public String PropRef { get; set; }

        [Column("trans_type")] public string Code { get; set; }

        [Column("post_date")] public DateTime Date { get; set; }

        [Key, Column("rtrans_sid")] public int Id { get; set; }

        [Column("batchno")] public Decimal batchno { get; set; }

        [Column("transno")] public int transno { get; set; }

        [Column("line_no")] public int line_no { get; set; }

        [Column("adjustment")] public Boolean adjustment { get; set; }

        [Column("apportion")] public Boolean apportion { get; set; }

        [Column("prop_deb")] public Boolean prop_deb { get; set; }

        [Column("none_rent")] public Boolean none_rent { get; set; }

        [Column("receipted")] public Boolean receipted { get; set; }

        [Column("line_segno")] public Decimal line_segno { get; set; }
        [Column("tag_ref")] public String tag_ref { get; set; }
        [Column("real_value")] public Decimal real_value { get; set; }
        [Column("full_value")] public Decimal full_value { get; set; }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UhTransaction) obj);
        }

        protected bool Equals(UhTransaction other)
        {
            return string.Equals(PropRef, other.PropRef) &&
                   string.Equals(Code, other.Code) &&
                   Date.Equals(other.Date) &&
                   Id == other.Id &&
                   batchno == other.batchno &&
                   transno == other.transno &&
                   line_no == other.line_no &&
                   adjustment == other.adjustment &&
                   apportion == other.apportion &&
                   prop_deb == other.prop_deb &&
                   none_rent == other.none_rent &&
                   receipted == other.receipted &&
                   line_segno == other.line_segno &&
                   tag_ref == other.tag_ref &&
                   real_value == other.real_value &&
                   full_value == other.full_value;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PropRef != null ? PropRef.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                hashCode = (hashCode * 397) ^ batchno.GetHashCode();
                hashCode = (hashCode * 397) ^ transno;
                hashCode = (hashCode * 397) ^ line_no;
                hashCode = (hashCode * 397) ^ adjustment.GetHashCode();
                hashCode = (hashCode * 397) ^ apportion.GetHashCode();
                hashCode = (hashCode * 397) ^ prop_deb.GetHashCode();
                hashCode = (hashCode * 397) ^ none_rent.GetHashCode();
                hashCode = (hashCode * 397) ^ receipted.GetHashCode();
                hashCode = (hashCode * 397) ^ line_segno.GetHashCode();
                hashCode = (hashCode * 397) ^ tag_ref.GetHashCode();
                hashCode = (hashCode * 397) ^ real_value.GetHashCode();
                hashCode = (hashCode * 397) ^ full_value.GetHashCode();

                return hashCode;
            }
        }
    }
}
