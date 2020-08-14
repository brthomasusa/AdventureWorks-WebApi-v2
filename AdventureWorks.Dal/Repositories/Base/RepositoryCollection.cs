using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Repositories.Purchasing;

namespace AdventureWorks.Dal.Repositories.Base
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private AdventureWorksContext _repoContext;

        private IEmployeeRepository _employee;

        private IVendorRepository _vendor;

        public RepositoryCollection(AdventureWorksContext context)
        {
            _repoContext = context;
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_repoContext);
                }

                return _employee;
            }
        }

        public IVendorRepository Vendor
        {
            get
            {
                if (_vendor == null)
                {
                    _vendor = new VendorRepository(_repoContext);
                }

                return _vendor;
            }
        }
    }
}