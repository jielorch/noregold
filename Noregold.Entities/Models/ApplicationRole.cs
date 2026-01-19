using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Noregold.Entities.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        [Column(TypeName = "VARCHAR(4000)")]
        public string? Description { get; set; }
    }
}
