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

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class VendorDeleteTests : RepoTestsBase
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorDeleteTests()
        {
            _vendorRepo = new VendorRepository(ctx);
        }

        [Fact]
        public void ShouldDeleteOneVendorBySettingIsActiveToFalse()
        {
            int vendorID = 5;
            var vendorDomainObj = _vendorRepo.GetVendorByID(vendorID);
            Assert.True(vendorDomainObj.IsActive);

            _vendorRepo.DeleteVendor(vendorDomainObj);

            var result = _vendorRepo.GetVendorByID(vendorID);
            Assert.False(vendorDomainObj.IsActive);
        }
    }
}