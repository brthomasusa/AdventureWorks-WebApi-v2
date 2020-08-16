using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class AddressRetrieveTests : RepoTestsBase
    {
        private readonly IAddressRepository _addressRepo;

        public AddressRetrieveTests()
        {
            _addressRepo = new AddressRepository(ctx);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 1)]
        public void ShouldLoadAddressDomainObjectsForEachVendor(int vendorID, int numberOfRecords)
        {
            var addressParams = new AddressParameters { PageNumber = 1, PageSize = 10 };
            var vendorAddressDomainObjs = _addressRepo.GetAddresses(vendorID, addressParams);
            int count = vendorAddressDomainObjs.Count();
            Assert.Equal(count, numberOfRecords);
        }

        [Theory]
        [InlineData(1, "28 San Marino Ct")]
        [InlineData(2, "90 Sunny Ave")]
        [InlineData(3, "298 Sunnybrook Drive")]
        [InlineData(4, "1900 Desoto Court")]
        [InlineData(5, "211 East Pleasant Run Rd")]
        [InlineData(6, "6266 Melody Lane")]
        public void ShouldGetAddressDomainObjByAddressID(int addressID, string line1)
        {
            var vendorAddressDomainObj = _addressRepo.GetAddressByID(addressID);
            Assert.Equal(line1, vendorAddressDomainObj.AddressLine1);
        }
    }
}