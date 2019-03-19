using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace transactionsapi.V1.Infrastructure
{
    [Table("rectype")]
    public class UhRecType
    {
        [Column("rec_code")] public String rec_code { get; set; }
        [Column("rec_desc")] public String RecDescription { get; set; }
        [Column("rec_hb")] public Boolean rec_rb { get; set; }
        [Column("rec_dd")] public Boolean rec_dd { get; set; }
        [Key, Column("rectype_sid")] public int Id { get; set; }
    }
}
