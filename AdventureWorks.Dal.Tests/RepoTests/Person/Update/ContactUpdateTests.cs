using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Update
{
    [Collection("AdventureWorks.Dal")]
    public class ContactUpdateTests : RepoTestsBase
    {
        private readonly IContactRepository _contactRepo;

        public ContactUpdateTests()
        {
            _contactRepo = new ContactRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldUpdateVendorContactUsingContactDomainObj()
        {
            var contactID = 8;
            var contact = await _contactRepo.GetContactByID(contactID);
            Assert.Equal("Dwayne", contact.MiddleName);
            Assert.Null(contact.Suffix);
            Assert.Equal("j.dough@adventure-works.com", contact.EmailAddress);

            contact.MiddleName = "Wayne";
            contact.Suffix = "Jr.";
            contact.EmailAddress = "j.w.dough@adventure-works.com";

            await _contactRepo.UpdateContact(contact);

            var result = await _contactRepo.GetContactByID(contactID);
            Assert.Equal("Wayne", result.MiddleName);
            Assert.Equal("Jr.", result.Suffix);
            Assert.Equal("j.w.dough@adventure-works.com", result.EmailAddress);
        }

        [Fact]
        public async Task ShouldRaiseExceptionContactWithInvalidParentEntityID()
        {
            var contactID = 8;
            var contact = await _contactRepo.GetContactByID(contactID);
            contact.ParentEntityID = 567;

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidEntityIdException>(() => _contactRepo.UpdateContact(contact));
            Assert.Equal("Error: Unable to determine the vendor that this contact is to be assigned to.", exception.Message);
        }

        [Fact]
        public async Task ShouldRaiseExceptionContactWithInvalidContactType()
        {
            var contactID = 8;
            var contact = await _contactRepo.GetContactByID(contactID);
            contact.ContactTypeID = 171;

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidContactTypeException>(() => _contactRepo.UpdateContact(contact));
            Assert.Equal("Error: Invalid contact type detected.", exception.Message);
        }
    }
}