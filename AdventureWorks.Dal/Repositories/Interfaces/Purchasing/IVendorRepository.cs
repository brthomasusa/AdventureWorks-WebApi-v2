using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Purchasing
{
    public interface IVendorRepository
    {
        Task<PagedList<VendorDomainObj>> GetVendors(VendorParameters vendorParameters);

        Task<VendorDomainObj> GetVendorByID(int businessEntityID);

        Task<VendorDomainObj> GetVendorWithDetails(int businessEntityID);

        void CreateVendor(VendorDomainObj vendor);

        void UpdateVendor(VendorDomainObj vendor);

        void DeleteVendor(VendorDomainObj vendor);
    }
}