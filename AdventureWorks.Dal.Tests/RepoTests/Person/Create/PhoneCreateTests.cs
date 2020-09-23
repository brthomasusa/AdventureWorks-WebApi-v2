using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Create
{
    [Collection("AdventureWorks.Dal")]
    public class PhoneCreateTests : RepoTestsBase
    {
        private readonly IPersonPhoneRepository _phoneRepo;

        public PhoneCreateTests()
        {
            _phoneRepo = new PersonPhoneRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldCreateOnePhoneRecord()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-9999",
                PhoneNumberTypeID = 2
            };

            await _phoneRepo.CreatePhone(phone);

            var result = await _phoneRepo.GetPhoneByID(phone.BusinessEntityID, phone.PhoneNumber, phone.PhoneNumberTypeID);
            Assert.Equal(phone.BusinessEntityID, result.BusinessEntityID);
            Assert.Equal(phone.PhoneNumber, result.PhoneNumber);
            Assert.Equal(phone.PhoneNumberTypeID, result.PhoneNumberTypeID);
        }

        [Fact]
        public async Task ShouldRaiseExceptionDuplicatePhoneRecord()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 3
            };

            var exception = await Assert.ThrowsAsync<AdventureWorksUniqueIndexException>(() => _phoneRepo.CreatePhone(phone));
            Assert.Equal("Error: This operation would result in a duplicate phone record.", exception.Message);
        }

        [Fact]
        public async Task ShouldRaiseExceptionInvalidBusinessEntityID()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 99,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 3
            };

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidEntityIdException>(() => _phoneRepo.CreatePhone(phone));
            Assert.Equal("Error: Unable to determine what person this phone number should be associated with.", exception.Message);
        }

        [Fact]
        public async Task ShouldRaiseExceptionInvalidPhoneNumberTypeID()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 73
            };

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidPhoneTypeException>(() => _phoneRepo.CreatePhone(phone));
            Assert.Equal("Error: The PhoneNumberTypeID '73' is not valid.", exception.Message);
        }
    }
}