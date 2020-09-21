using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Create
{
    [Collection("AdventureWorks.Dal")]
    public class VendorCreationTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorCreationTests()
        {
            _vendorRepo = new VendorRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldCreateVendorFromVendorDomainObj()
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

            var vendor = await _vendorRepo.GetVendorByID(vendorDomainObj.BusinessEntityID);
            Assert.NotNull(vendor);
            Assert.Equal("TESTVEN0001", vendor.AccountNumber);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateVendorAccountNumberWhileCreating()
        {
            var vendorDomainObj = new VendorDomainObj
            {
                AccountNumber = "CYCLERU0001",
                Name = "Test Vendor",
                CreditRating = CreditRating.Superior,
                PreferredVendor = true,
                IsActive = true
            };

            Action testCode = () =>
            {
                _vendorRepo.CreateVendor(vendorDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate vendor account number!", exception.Message);
        }
    }
}