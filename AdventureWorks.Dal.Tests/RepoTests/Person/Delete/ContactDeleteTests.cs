using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class ContactDeleteTests : RepoTestsBase
    {
        private readonly IContactRepository _contactRepo;

        public ContactDeleteTests()
        {
            _contactRepo = new ContactRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldDeleteOneVendorContactUsingContactDomainObj()
        {
            var contactID = 9;
            var contact = await _contactRepo.GetContactByID(contactID);

            Assert.NotNull(contact);

            _contactRepo.DeleteContact(contact);

            var result = await _contactRepo.GetContactByID(contactID);
            Assert.Null(result);
        }
    }
}