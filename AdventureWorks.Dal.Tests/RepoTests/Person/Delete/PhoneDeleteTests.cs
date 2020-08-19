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
            _phoneRepo = new PersonPhoneRepository(ctx);
        }

        [Fact]
        public void ShouldDeleteOnePhoneRecord()
        {
            var entityID = 1;
            var phoneNumber = "697-555-0142";
            var phoneTypeID = 1;

            var phone = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            Assert.NotNull(phone);

            _phoneRepo.DeletePhone(phone);

            var result = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            Assert.Null(result);
        }
    }
}