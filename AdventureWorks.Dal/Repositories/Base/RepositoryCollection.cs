using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Repositories.Person;

namespace AdventureWorks.Dal.Repositories.Base
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private AdventureWorksContext _repoContext;

        private IEmployeeRepository _employee;

        private IVendorRepository _vendor;

        private IPayHistoryRepository _payHistory;

        private IDepartmentHistoryRepository _deptHistory;

        private IPersonPhoneRepository _telephone;

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

        public IDepartmentHistoryRepository DepartmentHistory
        {
            get
            {
                if (_deptHistory == null)
                {
                    _deptHistory = new DepartmentHistoryRepository(_repoContext);
                }

                return _deptHistory;
            }
        }

        public IPayHistoryRepository PayHistory
        {
            get
            {
                if (_payHistory == null)
                {
                    _payHistory = new PayHistoryRepository(_repoContext);
                }

                return _payHistory;
            }
        }

        public IPersonPhoneRepository Telephone
        {
            get
            {
                if (_telephone == null)
                {
                    _telephone = new PersonPhoneRepository(_repoContext);
                }

                return _telephone;
            }
        }

    }
}