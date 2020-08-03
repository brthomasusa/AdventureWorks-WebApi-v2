using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Models.Base
{
    public abstract class EntityBase
    {
        [NotMapped]
        public List<Link> Links { get; set; } = new List<Link>();

        public Guid RowGuid { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}