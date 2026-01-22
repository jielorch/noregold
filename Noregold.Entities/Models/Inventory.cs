using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Noregold.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [Table("Invetories", Schema = "dbo")]
    public class Inventory : BaseModel<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string RFIDNo { get; set; } = null!;
    }
}
