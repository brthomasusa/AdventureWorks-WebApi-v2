using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.DomainModels;
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
            return PagedList<VendorDomainObj>.ToPagedList(FindAll(),
                vendorParameters.PageNumber,
                vendorParameters.PageSize);
        }

        public VendorDomainObj GetVendorByID(int businessEntityID)
        {
            return FindByCondition(vendor => vendor.BusinessEntityID == businessEntityID)
                .DefaultIfEmpty(new VendorDomainObj())
                .FirstOrDefault();
        }

        public void CreateVendor(VendorDomainObj vendorDomainObj)
        {
            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                //Create(vendor);
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