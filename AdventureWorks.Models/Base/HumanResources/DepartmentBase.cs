using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Base.HumanResources
{
    public class DepartmentBase : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DepartmentID { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }
    }
}