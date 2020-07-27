using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Models.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class VendorRepoTests : RepoTestsBase
    {
        private readonly IVendorRepo _vendorRepo;

        public VendorRepoTests()
        {
            _vendorRepo = new VendorRepo(ctx);
        }

        public override void Dispose()
        {
            _vendorRepo.Dispose();
        }

        [Fact]
        public void ShouldSetVendorIsActiveFlagToFalse()
        {
            var vendorID = 3;
            var vendor = _vendorRepo.Find(vendorID);
            Assert.NotNull(vendor);
            Assert.True(vendor.IsActive);

            _vendorRepo.Delete(vendor);

            var result = _vendorRepo.Find(vendorID);
            Assert.NotNull(result);
            Assert.False(result.IsActive);
        }

        [Fact]
        public void ShouldDeleteOneVendorAddress()
        {
            var addressID = 1;

            _vendorRepo.DeleteVendorAddress(addressID);

            var result = _vendorRepo.GetVendorAddress(1);
            Assert.Null(result);
        }

        [Fact]
        public void ShouldDeleteOneVendorBusinessEntityContact()
        {
            var vendorID = 7;
            var vendor = _vendorRepo.Find(vendorID);
            var bizEntityContact = vendor.BusinessEntityContacts
                .SingleOrDefault(bec => bec.BusinessEntityID == 7 && bec.PersonID == 9 && bec.ContactTypeID == 17);

            Assert.NotNull(bizEntityContact);

            _vendorRepo.DeleteVendorContact(bizEntityContact);

            var result = vendor.BusinessEntityContacts
                .SingleOrDefault(bec => bec.BusinessEntityID == 7 && bec.PersonID == 9 && bec.ContactTypeID == 17);

            Assert.Null(result);
        }
    }
}