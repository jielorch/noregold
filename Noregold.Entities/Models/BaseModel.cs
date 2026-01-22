using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Entities.Models
{
    public abstract class BaseModel<T>
    {
        public virtual T? Id { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
