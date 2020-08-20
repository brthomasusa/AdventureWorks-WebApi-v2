using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class PhoneRetrieveTests : RepoTestsBase
    {
        private readonly IPersonPhoneRepository _phoneRepo;

        public PhoneRetrieveTests()
        {
            _phoneRepo = new PersonPhoneRepository(ctx, logger);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(9, 1)]
        [InlineData(10, 2)]
        [InlineData(11, 1)]
        [InlineData(12, 1)]
        [InlineData(13, 1)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 1)]
        [InlineData(19, 1)]
        public void ShouldGetAllPhonesForEachBusinessEntityID(int entityID, int numberOfRecords)
        {
            var phoneParams = new PersonPhoneParameters { PageNumber = 1, PageSize = 10 };
            var phones = _phoneRepo.GetPhones(entityID, phoneParams);
            var count = phones.Count;

            Assert.Equal(count, numberOfRecords);
        }

        [Theory]
        [InlineData(1, "697-123-4567", 3)]
        [InlineData(9, "816-555-0142", 3)]
        [InlineData(10, "214-555-0100", 1)]
        public void ShouldGetOnePhoneRecordByPrimaryKeys(int entityID, string phoneNumber, int phoneTypeID)
        {
            var phone = _phoneRepo.GetPhoneByID(entityID, phoneNumber, phoneTypeID);
            Assert.NotNull(phone);
        }
    }
}