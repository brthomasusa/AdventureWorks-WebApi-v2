using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class PhoneDeleteTests : RepoTestsBase
    {
        private readonly IPersonPhoneRepository _phoneRepo;

        public PhoneDeleteTests()
        {
            _phoneRepo = new PersonPhoneRepository(ctx, logger);
        }

        [Fact]
        public void ShouldDeleteOnePhoneRecord()
        {
            int entityID = 1;
            var phoneNumber = "697-555-0142";
            var phoneTypeID = 1;

            var phone = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            Assert.NotNull(phone);

            _phoneRepo.DeletePhone(phone);

            var result = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            Assert.Null(result);
        }

        [Fact]
        public void ShouldFailToDeletePhoneWithNullEntityObjectException()
        {
            int entityID = 13;
            var phoneNumber = "273-555-0100";
            var phoneTypeID = 1;

            var phone = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            phone.BusinessEntityID = 300;

            Action testCode = () =>
            {
                _phoneRepo.DeletePhone(phone);
            };

            var exception = Assert.Throws<AdventureWorksNullEntityObjectException>(testCode);
            Assert.Equal("The phone record with ID: 300, number: 273-555-0100, and type: 1 could not be located in the database.", exception.Message);
        }
    }
}