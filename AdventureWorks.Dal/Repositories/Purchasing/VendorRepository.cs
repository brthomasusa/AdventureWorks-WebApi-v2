using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Purchasing
{
    public class VendorRepository : RepositoryBase<VendorDomainObj>, IVendorRepository
    {
        public VendorRepository(AdventureWorksContext context) : base(context) { }

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

        public void UpdateVendor(VendorDomainObj vendor)
        {
            Update(vendor);
        }

        public void DeleteVendor(VendorDomainObj vendor)
        {
            Delete(vendor);
        }
    }
}