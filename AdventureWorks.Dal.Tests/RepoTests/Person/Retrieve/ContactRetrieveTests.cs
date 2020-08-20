using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class ContactRetrieveTests : RepoTestsBase
    {
        private readonly IContactRepository _contactRepo;

        public ContactRetrieveTests()
        {
            _contactRepo = new ContactRepository(ctx, logger);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 3)]
        public void ShouldRetrieveAllContactDomainObjsForEachVendor(int vendorID, int numberOfContacts)
        {
            var contactDomainObjs = _contactRepo.GetContacts(vendorID, new ContactParameters { PageNumber = 1, PageSize = 10 });
            var count = contactDomainObjs.Count();
            Assert.Equal(count, numberOfContacts);
        }

        [Theory]
        [InlineData(8, "j.dough@adventure-works.com")]
        [InlineData(9, "jo3@adventure-works.com")]
        [InlineData(10, "terri.phide@adventure-works.com")]
        [InlineData(11, "marie0@adventure-works.com")]
        [InlineData(12, "william3@adventure-works.com")]
        [InlineData(13, "paula2@adventure-works.com")]
        [InlineData(19, "charelene0@adventure-works.com")]
        public void ShouldRetrieveEachVendorContactDomainObj(int contactID, string emailAddress)
        {
            var contactDomainObj = _contactRepo.GetContactByID(contactID);
            Assert.Equal(emailAddress, contactDomainObj.EmailAddress);
        }
    }
}