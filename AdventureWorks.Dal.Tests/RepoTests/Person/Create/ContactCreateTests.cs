using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Person;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Person.Create
{
    [Collection("AdventureWorks.Dal")]
    public class ContactCreateTests : RepoTestsBase
    {
        private readonly IContactRepository _contactRepo;

        public ContactCreateTests()
        {
            _contactRepo = new ContactRepository(ctx, logger);
        }

        [Fact]
        public void ShouldCreateOneVendorContactUsingContactDomainObj()
        {
            var contact = new ContactDomainObj
            {
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jennifer",
                LastName = "Lopez",
                EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                EmailAddress = "j.lopez@adventure-works.com",
                EmailPasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                EmailPasswordSalt = "K4sEsXg=",
                ContactTypeID = 17,
                ParentEntityID = 2
            };

            _contactRepo.CreateContact(contact);

            var result = _contactRepo.GetContactByID(contact.BusinessEntityID);
            Assert.NotNull(result);
            Assert.Equal(contact.EmailPasswordHash, result.EmailPasswordHash);
        }

        [Fact]
        public void ShouldRaiseExceptionContactDomainObjWithInvalidParentEntityID()
        {
            var contact = new ContactDomainObj
            {
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jennifer",
                LastName = "Lopez",
                EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                EmailAddress = "j.lopez@adventure-works.com",
                EmailPasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                EmailPasswordSalt = "K4sEsXg=",
                ContactTypeID = 17,
                ParentEntityID = 42
            };

            Action testCode = () =>
            {
                _contactRepo.CreateContact(contact);
            };

            var exception = Assert.Throws<AdventureWorksInvalidEntityIdException>(testCode);
            Assert.Equal("Error: Unable to determine the vendor that this contact is to be assigned to.", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionContactDomainObjWithInvalidContactType()
        {
            var contact = new ContactDomainObj
            {
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jennifer",
                LastName = "Lopez",
                EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                EmailAddress = "j.lopez@adventure-works.com",
                EmailPasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                EmailPasswordSalt = "K4sEsXg=",
                ContactTypeID = 171,
                ParentEntityID = 2
            };

            Action testCode = () =>
            {
                _contactRepo.CreateContact(contact);
            };

            var exception = Assert.Throws<AdventureWorksInvalidContactTypeException>(testCode);
            Assert.Equal("Error: Invalid contact type detected.", exception.Message);
        }
    }
}