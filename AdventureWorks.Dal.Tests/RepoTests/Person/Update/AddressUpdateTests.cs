using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Update
{
    [Collection("AdventureWorks.Dal")]
    public class AddressUpdateTests : RepoTestsBase
    {
        private readonly IAddressRepository _addressRepo;

        public AddressUpdateTests()
        {
            _addressRepo = new AddressRepository(ctx, logger);
        }

        [Fact]
        public void ShouldUpdateVendorAddressUsingAddressDomainObj()
        {
            var addressID = 6;
            var addressdomainObj = _addressRepo.GetAddressByID(addressID);
            Assert.Null(addressdomainObj.AddressLine2);

            addressdomainObj.AddressLine2 = "Suite ZZZ";
            _addressRepo.UpdateAddress(addressdomainObj);

            var result = _addressRepo.GetAddressByID(addressID);
            Assert.Equal("Suite ZZZ", result.AddressLine2);
        }

        [Fact]
        public void ShouldRaiseExceptionForDuplicateEntityAddress()
        {
            var addressID = 6;
            var addressdomainObj = _addressRepo.GetAddressByID(addressID);

            Assert.NotNull(addressdomainObj);

            addressdomainObj.AddressLine1 = "9435 Breck Court";
            addressdomainObj.City = "Bellevue";
            addressdomainObj.StateProvinceID = 79;
            addressdomainObj.PostalCode = "98004";

            Action testCode = () =>
            {
                _addressRepo.UpdateAddress(addressdomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: There is an existing entity with this address!", exception.Message);
        }
    }
}