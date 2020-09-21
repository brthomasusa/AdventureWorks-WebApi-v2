using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Update
{
    [Collection("AdventureWorks.Dal")]
    public class PhoneUpdateTests : RepoTestsBase
    {
        private readonly IPersonPhoneRepository _phoneRepo;

        public PhoneUpdateTests()
        {
            _phoneRepo = new PersonPhoneRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldUpdateOnePhoneRecordByCallingCreate()
        {
            var phone = new PersonPhone
            {
                BusinessEntityID = 9,
                PhoneNumber = "816-555-9999",
                PhoneNumberTypeID = 2
            };

            _phoneRepo.UpdatePhone(phone);

            var result = await _phoneRepo.GetPhoneByID(phone.BusinessEntityID, phone.PhoneNumber, phone.PhoneNumberTypeID);
            Assert.Equal(phone.BusinessEntityID, result.BusinessEntityID);
            Assert.Equal(phone.PhoneNumber, result.PhoneNumber);
            Assert.Equal(phone.PhoneNumberTypeID, result.PhoneNumberTypeID);
        }
    }
}