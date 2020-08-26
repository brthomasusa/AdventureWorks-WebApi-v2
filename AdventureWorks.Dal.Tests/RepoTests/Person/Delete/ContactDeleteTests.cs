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
        public void ShouldDeleteOneVendorContactUsingContactDomainObj()
        {
            var contactID = 9;
            var contact = _contactRepo.GetContactByID(contactID);

            Assert.NotNull(contact);

            _contactRepo.DeleteContact(contact);

            var result = _contactRepo.GetContactByID(contactID);
            Assert.Null(result);
        }
    }
}