using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Purchasing
{
    public interface IVendorRepository : IRepositoryBase<VendorDomainObj>
    {
        PagedList<VendorDomainObj> GetVendors(VendorParameters vendorParameters);

        VendorDomainObj GetVendorByID(int businessEntityID);

        void CreateVendor(VendorDomainObj vendor);

        void UpdateVendor(VendorDomainObj vendor);

        void DeleteVendor(VendorDomainObj vendor);
    }
}