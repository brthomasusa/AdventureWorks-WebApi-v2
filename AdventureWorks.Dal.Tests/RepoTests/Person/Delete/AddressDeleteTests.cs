using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class AddressDeleteTests : RepoTestsBase
    {
        private readonly IAddressRepository _addressRepo;

        public AddressDeleteTests()
        {
            _addressRepo = new AddressRepository(ctx, logger);
        }

        [Fact]
        public void ShouldDeleteOneVendorAddressUsingAddressDomainObj()
        {
            var vendorID = 3;
            var addressParams = new AddressParameters { PageNumber = 1, PageSize = 10 };
            var vendorAddressDomainObjs = _addressRepo.GetAddresses(vendorID, addressParams);
            var count = vendorAddressDomainObjs.Count;

            Assert.Equal(2, count);

            _addressRepo.DeleteAddress(vendorAddressDomainObjs[0]);
            vendorAddressDomainObjs = _addressRepo.GetAddresses(vendorID, addressParams);

            Assert.Equal(count - 1, vendorAddressDomainObjs.Count);
        }

        [Fact]
        public void ShouldRaiseExceptionWhileDeletingAddressWithInvalidAddressID()
        {
            var addressID = 6;
            var addressDomainObj = _addressRepo.GetAddressByID(addressID);
            addressDomainObj.AddressID = 99;

            Action testCode = () =>
            {
                _addressRepo.DeleteAddress(addressDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksNullEntityObjectException>(testCode);
            Assert.Equal("Error: Update failed; unable to locate an address in the database with ID '99'.", exception.Message);
        }
    }
}