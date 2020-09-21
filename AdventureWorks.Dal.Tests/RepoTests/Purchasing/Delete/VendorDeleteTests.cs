using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class VendorDeleteTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorDeleteTests()
        {
            _vendorRepo = new VendorRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldDeleteOneVendorBySettingIsActiveToFalse()
        {
            int vendorID = 5;
            var vendorDomainObj = await _vendorRepo.GetVendorByID(vendorID);
            Assert.True(vendorDomainObj.IsActive);

            _vendorRepo.DeleteVendor(vendorDomainObj);

            var result = await _vendorRepo.GetVendorByID(vendorID);
            Assert.False(vendorDomainObj.IsActive);
        }


        [Fact]
        public async Task ShouldRaiseExceptionUnableToLocateVendorWithIDWhileDeleting()
        {
            int vendorID = 5;
            var vendorDomainObj = await _vendorRepo.GetVendorByID(vendorID);
            vendorDomainObj.BusinessEntityID = 500;

            Action testCode = () =>
            {
                _vendorRepo.DeleteVendor(vendorDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksNullEntityObjectException>(testCode);
            Assert.Equal("Error: Failed to change vendor status to in-active. Unable to locate a vendor in the database with ID '500'.", exception.Message);
        }
    }
}