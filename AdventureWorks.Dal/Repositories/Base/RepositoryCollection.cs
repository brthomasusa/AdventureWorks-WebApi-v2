using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Repositories.Utility;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Base
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private AdventureWorksContext _repoContext;

        private ILoggerManager _logger;

        private IEmployeeRepository _employee;

        private IVendorRepository _vendor;

        private IPayHistoryRepository _payHistory;

        private IDepartmentHistoryRepository _deptHistory;

        private IPersonPhoneRepository _telephone;

        private IDepartmentRepository _department;

        private IAddressRepository _address;

        private IContactRepository _contact;

        private ILookupRepository _lookup;

        public RepositoryCollection(AdventureWorksContext context, ILoggerManager logger)
        {
            _repoContext = context;
            _logger = logger;
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_repoContext, _logger);
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
                    _vendor = new VendorRepository(_repoContext, _logger);
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
                    _deptHistory = new DepartmentHistoryRepository(_repoContext, _logger);
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
                    _payHistory = new PayHistoryRepository(_repoContext, _logger);
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
                    _telephone = new PersonPhoneRepository(_repoContext, _logger);
                }

                return _telephone;
            }
        }

        public IDepartmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new DepartmentRepository(_repoContext, _logger);
                }

                return _department;
            }
        }

        public IAddressRepository Address
        {
            get
            {
                if (_address == null)
                {
                    _address = new AddressRepository(_repoContext, _logger);
                }

                return _address;
            }
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = new ContactRepository(_repoContext, _logger);
                }

                return _contact;
            }
        }

        public ILookupRepository Lookup
        {
            get
            {
                if (_lookup == null)
                {
                    _lookup = new LookupRepository(_repoContext, _logger);
                }

                return _lookup;
            }
        }
    }
}