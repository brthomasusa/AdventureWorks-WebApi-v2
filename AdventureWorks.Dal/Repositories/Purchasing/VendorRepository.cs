using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Purchasing
{
    public class VendorRepository : RepositoryBase<VendorDomainObj>, IVendorRepository
    {
        private const string CLASSNAME = "VendorRepository";

        public VendorRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<VendorDomainObj> GetVendors(VendorParameters vendorParameters)
        {
            return PagedList<VendorDomainObj>.ToPagedList(DbContext.VendorDomainObj.AsQueryable(),
                vendorParameters.PageNumber,
                vendorParameters.PageSize);
        }

        public VendorDomainObj GetVendorByID(int businessEntityID)
        {
            return DbContext.VendorDomainObj.Where(vendor => vendor.BusinessEntityID == businessEntityID)
                .AsQueryable()
                .FirstOrDefault();
        }

        public VendorDomainObj GetVendorWithDetails(int businessEntityID)
        {
            var vendor = DbContext.VendorDomainObj.Where(vendor => vendor.BusinessEntityID == businessEntityID)
                .AsQueryable()
                .FirstOrDefault();

            vendor.Addresses.AddRange(DbContext.AddressDomainObj.Where(a => a.ParentEntityID == vendor.BusinessEntityID).ToList());
            vendor.Contacts.AddRange(DbContext.ContactDomainObj.Where(c => c.ParentEntityID == vendor.BusinessEntityID).ToList());

            return vendor;
        }

        public void CreateVendor(VendorDomainObj vendorDomainObj)
        {
            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var bizEntity = new BusinessEntity { };
                DbContext.BusinessEntity.Add(bizEntity);
                Save();

                vendorDomainObj.BusinessEntityID = bizEntity.BusinessEntityID;
                var vendor = new Vendor { };
                vendor.Map(vendorDomainObj);
                DbContext.Vendor.Add(vendor);
                Save();
            }
        }

        public void UpdateVendor(VendorDomainObj vendorDomainObj)
        {
            var vendor = DbContext.Vendor.Find(vendorDomainObj.BusinessEntityID);

            if (vendor == null)
            {
                string msg = $"Error: Update failed; unable to locate a vendor in the database with ID '{vendorDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateVendor " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            vendor.Map(vendorDomainObj);
            DbContext.Vendor.Update(vendor);
            Save();
        }

        public void DeleteVendor(VendorDomainObj vendorDomainObj)
        {
            // There is a tsql trigger that prevents deletion of vendors!
            vendorDomainObj.IsActive = false;
            var vendor = DbContext.Vendor.Find(vendorDomainObj.BusinessEntityID);

            if (vendor == null)
            {
                string msg = $"Error: Failed to change vendor status to in-active. Unable to locate a vendor in the database with ID '{vendorDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteVendor " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            vendor.Map(vendorDomainObj);
            DbContext.Vendor.Update(vendor);
            Save();
        }
    }
}