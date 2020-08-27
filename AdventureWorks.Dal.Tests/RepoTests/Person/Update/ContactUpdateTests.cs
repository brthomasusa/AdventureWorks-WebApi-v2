using System;
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
        public void ShouldUpdateVendorContactUsingContactDomainObj()
        {
            var contactID = 8;
            var contact = _contactRepo.GetContactByID(contactID);
            Assert.Equal("Dwayne", contact.MiddleName);
            Assert.Null(contact.Suffix);
            Assert.Equal("j.dough@adventure-works.com", contact.EmailAddress);

            contact.MiddleName = "Wayne";
            contact.Suffix = "Jr.";
            contact.EmailAddress = "j.w.dough@adventure-works.com";

            _contactRepo.UpdateContact(contact);

            var result = _contactRepo.GetContactByID(contactID);
            Assert.Equal("Wayne", result.MiddleName);
            Assert.Equal("Jr.", result.Suffix);
            Assert.Equal("j.w.dough@adventure-works.com", result.EmailAddress);
        }

        [Fact]
        public void ShouldRaiseExceptionContactWithInvalidParentEntityID()
        {
            var contactID = 8;
            var contact = _contactRepo.GetContactByID(contactID);
            contact.ParentEntityID = 567;

            Action testCode = () =>
            {
                _contactRepo.UpdateContact(contact);
            };

            var exception = Assert.Throws<AdventureWorksInvalidEntityIdException>(testCode);
            Assert.Equal("Error: Unable to determine the vendor that this contact is to be assigned to.", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionContactWithInvalidContactType()
        {
            var contactID = 8;
            var contact = _contactRepo.GetContactByID(contactID);
            contact.ContactTypeID = 171;

            Action testCode = () =>
            {
                _contactRepo.UpdateContact(contact);
            };

            var exception = Assert.Throws<AdventureWorksInvalidContactTypeException>(testCode);
            Assert.Equal("Error: Invalid contact type detected.", exception.Message);
        }
    }
}