using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using PersonClass = AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Create
{
    [Collection("AdventureWorks.Dal")]
    public class VendorCreationTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorCreationTests()
        {
            _vendorRepo = new VendorRepository(ctx);
        }

        [Fact]
        public void ShouldCreateVendorFromVendorDomainObj()
        {
            var vendorDomainObj = new VendorDomainObj
            {
                AccountNumber = "TESTVEN0001",
                Name = "Test Vendor",
                CreditRating = CreditRating.Superior,
                PreferredVendor = true,
                IsActive = true
            };

            _vendorRepo.CreateVendor(vendorDomainObj);

            var vendor = _vendorRepo.GetVendorByID(vendorDomainObj.BusinessEntityID);
            Assert.NotNull(vendor);
            Assert.Equal("TESTVEN0001", vendor.AccountNumber);
        }
    }
}