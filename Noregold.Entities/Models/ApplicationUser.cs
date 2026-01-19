using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Noregold.Entities.Models
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid UserUId { get; set; }
        [Column(TypeName = "NVARCHAR(255)")]
        public string FirstName { get; set; } = null!;
        [Column(TypeName = "NVARCHAR(255)")]
        public string LastName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? Gender { get; set; }
        public string? PictureUrl { get; set; }
    }
}
