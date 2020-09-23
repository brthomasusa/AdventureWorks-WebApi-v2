using System;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using PersonClass = AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Update
{
    [Collection("AdventureWorks.Dal")]
    public class VendorUpdateTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorUpdateTests()
        {
            _vendorRepo = new VendorRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldUpdateOneVendorViaVendorDomainObj()
        {
            int vendorID = 5;
            var vendorDomainObj = await _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("Light Speed", vendorDomainObj.Name);

            vendorDomainObj.Name = "Dark Speed";
            await _vendorRepo.UpdateVendor(vendorDomainObj);

            var result = await _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("Dark Speed", result.Name);
        }

        [Fact]
        public async Task ShouldRaiseExceptionBecauseOfInvalidVendorID()
        {
            int vendorID = 5;
            var vendorDomainObj = await _vendorRepo.GetVendorByID(vendorID);
            vendorDomainObj.BusinessEntityID = 500;

            var exception = await Assert.ThrowsAsync<AdventureWorksNullEntityObjectException>(() => _vendorRepo.UpdateVendor(vendorDomainObj));
            Assert.Equal("Error: Update failed; unable to locate a vendor in the database with ID '500'.", exception.Message);
        }

        [Fact]
        public async Task ShouldRaiseExceptionDuplicateVendorAccountNumberWhileUpdating()
        {
            int vendorID = 5;
            var vendorDomainObj = await _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("LIGHTSP0001", vendorDomainObj.AccountNumber);

            vendorDomainObj.AccountNumber = "DFWBIRE0001";

            var exception = await Assert.ThrowsAsync<AdventureWorksUniqueIndexException>(() => _vendorRepo.UpdateVendor(vendorDomainObj));
            Assert.Equal("Error: This operation would result in a duplicate vendor account number!", exception.Message);
        }
    }
}