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
            _phoneRepo = new PersonPhoneRepository(ctx);
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
            Assert.NotNull(result);
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
            Assert.Equal("Error: This operation would result in a duplicate phone record!", exception.Message);
        }
    }
}