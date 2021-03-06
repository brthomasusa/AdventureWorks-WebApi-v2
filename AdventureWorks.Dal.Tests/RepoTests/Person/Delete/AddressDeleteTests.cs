using System;
using System.Threading.Tasks;
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
        public async Task ShouldDeleteOneVendorAddressUsingAddressDomainObj()
        {
            var vendorID = 3;
            var addressParams = new AddressParameters { PageNumber = 1, PageSize = 10 };
            var vendorAddressDomainObjs = await _addressRepo.GetAddresses(vendorID, addressParams);
            var count = vendorAddressDomainObjs.Count;

            Assert.Equal(2, count);

            await _addressRepo.DeleteAddress(vendorAddressDomainObjs[0]);
            vendorAddressDomainObjs = await _addressRepo.GetAddresses(vendorID, addressParams);

            Assert.Equal(count - 1, vendorAddressDomainObjs.Count);
        }

        [Fact]
        public async Task ShouldRaiseExceptionAtemptingToDeleteAddressUsingInvalidAddressID()
        {
            var addressID = 6;
            var addressDomainObj = await _addressRepo.GetAddressByID(addressID);
            addressDomainObj.AddressID = 99;

            var exception = await Assert.ThrowsAsync<AdventureWorksNullEntityObjectException>(() => _addressRepo.DeleteAddress(addressDomainObj));
            Assert.Equal("Error: Update failed; unable to locate an address in the database with ID '99'.", exception.Message);
        }
    }
}