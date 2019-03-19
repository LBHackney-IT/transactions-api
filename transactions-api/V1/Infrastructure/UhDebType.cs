using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace transactionsapi.V1.Infrastructure
{
[Table("debtype")]
public class UhDebType
{
    [Column("deb_desc")] public String DebDescription { get; set; }
    [Column("deb_code")] public String deb_code { get; set; }
    [Key, Column("debtype_sid")] public int Id { get; set; }
}
}
