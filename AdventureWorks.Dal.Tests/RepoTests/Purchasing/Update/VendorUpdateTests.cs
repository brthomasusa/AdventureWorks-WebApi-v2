using System;
using System.Linq;
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
            _vendorRepo = new VendorRepository(ctx);
        }

        [Fact]
        public void ShouldUpdateOneVendorViaVendorDomainObj()
        {
            int vendorID = 5;
            var vendorDomainObj = _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("Light Speed", vendorDomainObj.Name);

            vendorDomainObj.Name = "Dark Speed";
            _vendorRepo.UpdateVendor(vendorDomainObj);

            var result = _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("Dark Speed", result.Name);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateVendorAccountNumberWhileUpdating()
        {
            int vendorID = 5;
            var vendorDomainObj = _vendorRepo.GetVendorByID(vendorID);
            Assert.Equal("LIGHTSP0001", vendorDomainObj.AccountNumber);

            vendorDomainObj.AccountNumber = "DFWBIRE0001";

            Action testCode = () =>
            {
                _vendorRepo.UpdateVendor(vendorDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate vendor account number!", exception.Message);
        }
    }
}