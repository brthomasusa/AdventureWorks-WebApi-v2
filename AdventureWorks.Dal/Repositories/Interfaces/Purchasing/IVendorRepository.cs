using System.Threading.Tasks;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Purchasing
{
    public interface IVendorRepository
    {
        Task<PagedList<VendorDomainObj>> GetVendors(VendorParameters vendorParameters);

        Task<VendorDomainObj> GetVendorByID(int businessEntityID);

        Task<VendorDomainObj> GetVendorWithDetails(int businessEntityID);

        Task CreateVendor(VendorDomainObj vendor);

        Task UpdateVendor(VendorDomainObj vendor);

        Task DeleteVendor(VendorDomainObj vendor);
    }
}