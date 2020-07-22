using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class DepartmentRepo : RepoBase<Department>, IDepartmentRepo
    {
        public DepartmentRepo(AdventureWorksContext context) : base(context) { }

        public DepartmentRepo(DbContextOptions<AdventureWorksContext> options ) : base(options) { }        
    }
}