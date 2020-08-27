using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Create
{
    [Collection("AdventureWorks.Dal")]
    public class AddressCreateTests : RepoTestsBase
    {
        private readonly IAddressRepository _addressRepo;

        public AddressCreateTests()
        {
            _addressRepo = new AddressRepository(ctx, logger);
        }

        [Fact]
        public void ShouldCreateOneAddressUsingAddressDomainObj()
        {
            var vendorID = 4;
            var addressDomainObj = new AddressDomainObj
            {
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = vendorID
            };

            _addressRepo.CreateAddress(addressDomainObj);

            var result = _addressRepo.GetAddressByID(addressDomainObj.AddressID);
            Assert.Equal(addressDomainObj.AddressLine1, result.AddressLine1);

            var addressDomainObjs = _addressRepo.GetAddresses(vendorID, new AddressParameters { PageNumber = 1, PageSize = 10 });
            Assert.Equal(addressDomainObjs[0].AddressLine1, addressDomainObj.AddressLine1);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateAddressInAddressDomainObj()
        {
            var vendorID = 4;
            var addressDomainObj = new AddressDomainObj
            {
                AddressLine1 = "28 San Marino Ct",
                City = "Bellingham",
                StateProvinceID = 79,
                PostalCode = "98225",
                AddressTypeID = 3,
                ParentEntityID = vendorID
            };

            Action testCode = () =>
            {
                _addressRepo.CreateAddress(addressDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: There is an existing entity with this address!", exception.Message);
        }
    }
}