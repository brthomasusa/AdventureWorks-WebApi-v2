using System;
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
        public void ShouldCreateOnePhoneRecord()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-9999",
                PhoneNumberTypeID = 2
            };

            _phoneRepo.CreatePhone(phone);

            var result = _phoneRepo.GetPhoneByID(phone.BusinessEntityID, phone.PhoneNumber, phone.PhoneNumberTypeID);
            Assert.Equal(phone.BusinessEntityID, result.BusinessEntityID);
            Assert.Equal(phone.PhoneNumber, result.PhoneNumber);
            Assert.Equal(phone.PhoneNumberTypeID, result.PhoneNumberTypeID);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicatePhoneRecord()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 3
            };

            Action testCode = () =>
            {
                _phoneRepo.CreatePhone(phone);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate phone record.", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionInvalidBusinessEntityID()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 99,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 3
            };

            Action testCode = () =>
            {
                _phoneRepo.CreatePhone(phone);
            };

            var exception = Assert.Throws<AdventureWorksInvalidEntityIdException>(testCode);
            Assert.Equal("Error: Unable to determine what person this phone number should be associated with.", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionInvalidPhoneNumberTypeID()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-0142",
                PhoneNumberTypeID = 73
            };

            Action testCode = () =>
            {
                _phoneRepo.CreatePhone(phone);
            };

            var exception = Assert.Throws<AdventureWorksInvalidPhoneTypeException>(testCode);
            Assert.Equal("Error: The PhoneNumberTypeID '73' is not valid.", exception.Message);
        }
    }
}