using System.Linq;
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
            _vendorRepo = new VendorRepository(ctx);
        }

        [Fact]
        public void ShouldGetAllVendorDomainObjsUsingRepositoryBase()
        {
            var vendorParams = new VendorParameters { PageNumber = 1, PageSize = 10 };
            var vendors = _vendorRepo.GetVendors(vendorParams);
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
        public void GetEachVendorDomainObjByID(int vendorID, string acctNumber)
        {
            var vendor = _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal(acctNumber, vendor.AccountNumber);
        }
    }
}