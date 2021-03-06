using System;
using System.Threading.Tasks;
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
        public async Task ShouldCreateOneVendorContactUsingContactDomainObj()
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

            await _contactRepo.CreateContact(contact);

            var result = await _contactRepo.GetContactByID(contact.BusinessEntityID);
            Assert.NotNull(result);
            Assert.Equal(contact.EmailPasswordHash, result.EmailPasswordHash);
        }

        [Fact]
        public async Task ShouldRaiseExceptionContactDomainObjWithInvalidParentEntityID()
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

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidEntityIdException>(() => _contactRepo.CreateContact(contact));
            Assert.Equal("Error: Unable to determine the vendor that this contact is to be assigned to.", exception.Message);
        }

        [Fact]
        public async Task ShouldRaiseExceptionContactDomainObjWithInvalidContactType()
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

            var exception = await Assert.ThrowsAsync<AdventureWorksInvalidContactTypeException>(() => _contactRepo.CreateContact(contact));
            Assert.Equal("Error: Invalid contact type detected.", exception.Message);
        }
    }
}