using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedList<VendorDomainObj>> GetVendors(VendorParameters vendorParameters)
        {
            Func<VendorDomainObj, bool> filterCriteria;
            var vendorStatus = vendorParameters.VendorStatus.ToUpper();

            if (vendorStatus == "ACTIVE")
            {
                filterCriteria = ee => ee.IsActive == true;
            }
            else if (vendorStatus == "INACTIVE")
            {
                filterCriteria = ee => ee.IsActive == false;
            }
            else
            {
                filterCriteria = ee => ee.IsActive == true || ee.IsActive == false;
            }

            var vendorList = DbContext.VendorDomainObj.Where(filterCriteria).AsQueryable();

            SearchByName(ref vendorList, vendorParameters.Name);

            return await PagedList<VendorDomainObj>.ToPagedList(
                vendorList.OrderBy(v => v.Name),
                vendorParameters.PageNumber,
                vendorParameters.PageSize);
        }

        public async Task<VendorDomainObj> GetVendorByID(int businessEntityID)
        {
            return await DbContext.VendorDomainObj
                .Where(vendor => vendor.BusinessEntityID == businessEntityID)
                .FirstOrDefaultAsync();
        }

        public async Task<VendorDomainObj> GetVendorWithDetails(int businessEntityID)
        {
            var vendor = await DbContext.VendorDomainObj
                .Where(vendor => vendor.BusinessEntityID == businessEntityID)
                .FirstOrDefaultAsync();

            vendor.Addresses.AddRange(await DbContext.AddressDomainObj.Where(a => a.ParentEntityID == vendor.BusinessEntityID).ToListAsync());
            vendor.Contacts.AddRange(await DbContext.ContactDomainObj.Where(c => c.ParentEntityID == vendor.BusinessEntityID).ToListAsync());

            return vendor;
        }

        public async Task CreateVendor(VendorDomainObj vendorDomainObj)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    var bizEntity = new BusinessEntity { };
                    DbContext.BusinessEntity.Add(bizEntity);
                    await Save();

                    vendorDomainObj.BusinessEntityID = bizEntity.BusinessEntityID;
                    var vendor = new Vendor { };
                    vendor.Map(vendorDomainObj);
                    DbContext.Vendor.Add(vendor);
                    await Save();

                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    RepoLogger.LogError($" {CLASSNAME}.CreateVendor {ex.Message}");
                }
            }
        }

        public async Task UpdateVendor(VendorDomainObj vendorDomainObj)
        {
            var vendor = await DbContext.Vendor.FindAsync(vendorDomainObj.BusinessEntityID);

            if (vendor == null)
            {
                string msg = $"Error: Update failed; unable to locate a vendor in the database with ID '{vendorDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateVendor " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            vendor.Map(vendorDomainObj);
            DbContext.Vendor.Update(vendor);
            await Save();
        }

        public async Task DeleteVendor(VendorDomainObj vendorDomainObj)
        {
            // There is a tsql trigger that prevents deletion of vendors!
            vendorDomainObj.IsActive = false;
            var vendor = await DbContext.Vendor.FindAsync(vendorDomainObj.BusinessEntityID);

            if (vendor == null)
            {
                string msg = $"Error: Failed to change vendor status to in-active. Unable to locate a vendor in the database with ID '{vendorDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteVendor " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            vendor.Map(vendorDomainObj);
            DbContext.Vendor.Update(vendor);
            await Save();
        }

        private void SearchByName(ref IQueryable<VendorDomainObj> vendors, string vendorName)
        {
            if (!vendors.Any() || string.IsNullOrWhiteSpace(vendorName))
            {
                return;
            }

            vendors = vendors.Where(v => v.Name.ToLower().Contains(vendorName.Trim().ToLower()));
        }
    }
}