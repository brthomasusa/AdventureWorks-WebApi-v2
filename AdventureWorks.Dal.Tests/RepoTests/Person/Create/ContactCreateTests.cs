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
    }
}