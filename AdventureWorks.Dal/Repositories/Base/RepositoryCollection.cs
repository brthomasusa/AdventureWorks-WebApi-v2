using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Models.DomainModels;

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

        public void Save()
        {

            try
            {
                _repoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var msg = "Error: The vendor you are trying to update does not exist. Try refreshing your screen.";

                throw new AdventureWorksConcurrencyExeception(msg, ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new AdventureWorksRetryLimitExceededException("There is a problem with your connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains("AK_Vendor_AccountNumber", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate vendor account number!", ex);
                    }
                    else if (sqlException.Message.Contains("IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: There is an existing entity with this address!", ex);
                    }
                }

                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
        }
    }
}