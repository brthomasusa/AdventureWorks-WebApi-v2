using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class VendorRetrievalTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorRetrievalTests()
        {
            _vendorRepo = new VendorRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldGetAllVendorDomainObjsUsingRepositoryBase()
        {
            var vendorParams = new VendorParameters { PageNumber = 1, PageSize = 10 };
            var vendors = await _vendorRepo.GetVendors(vendorParams);
            var count = vendors.Count();
            Assert.Equal(6, count);
        }

        [Theory]
        [InlineData(2, "CYCLERU0001")]
        [InlineData(3, "DESOTOB0001")]
        [InlineData(4, "DFWBIRE0001")]
        [InlineData(5, "LIGHTSP0001")]
        [InlineData(6, "TRIKES0001")]
        [InlineData(7, "AUSTRALI0001")]
        public async Task GetEachVendorDomainObjByID(int vendorID, string acctNumber)
        {
            var vendor = await _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal(acctNumber, vendor.AccountNumber);
        }

        [Theory]
        [InlineData(2, 1, 0)]
        [InlineData(3, 2, 1)]
        [InlineData(4, 0, 1)]
        [InlineData(5, 1, 1)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 3)]
        public async Task GetEachVendorDomainObjByIdWithDetails(int vendorID, int numberOfAddresses, int numberOfContacts)
        {
            var vendor = await _vendorRepo.GetVendorWithDetails(vendorID);
            var addressCount = vendor.Addresses.Count;
            var contactCount = vendor.Contacts.Count;

            Assert.Equal(addressCount, numberOfAddresses);
            Assert.Equal(contactCount, numberOfContacts);
        }
    }
}