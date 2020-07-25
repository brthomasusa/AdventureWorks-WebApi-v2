using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using PersonClass = AdventureWorks.Models.Person.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Create
{
    [Collection("AdventureWorks.Dal")]
    public class VendorRepoTests : RepoTestsBase
    {
        private readonly IVendorRepo _vendorRepo;

        public VendorRepoTests()
        {
            _vendorRepo = new VendorRepo(ctx);
        }

        public override void Dispose()
        {
            _vendorRepo.Dispose();
        }

        [Fact]
        public void ShouldAddOneVendorWithoutAddressOrContacts()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendor = new Vendor
                {
                    AccountNumber = "TESTVEN0001",
                    Name = "Test Vendor",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true
                };

                var bizEntityID = _vendorRepo.Add(vendor);

                var result = _vendorRepo.Find(bizEntityID);

                Assert.NotNull(result);
            }
        }

        [Fact]
        public void ShouldAddOneVendorWithAddressAndContact()
        {
            var contact = new PersonClass
            {
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jane",
                MiddleName = "Jamie",
                LastName = "Doe",
                EmailPromotion = EmailPromoPreference.AdventureWorksOnly,
                EmailAddressObj = new EmailAddress
                {
                    PersonEmailAddress = "jane.doe@adventure-works.com"
                },
                PasswordObj = new PersonPWord
                {
                    PasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                    PasswordSalt = "d2tgUmM="
                },
                Phones =
                {
                    new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 3}
                }
            };

            var address = new Address
            {
                AddressLine1 = "123 Sunny Ave",
                City = "Berkeley",
                PostalCode = "94704",
                StateProvinceID = 9,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 3
                }
            };

            var vendor = new Vendor
            {
                AccountNumber = "TESTVEN0001",
                Name = "Test Vendor",
                CreditRating = CreditRating.Superior,
                PreferredVendor = true,
                IsActive = true
            };

            var vendorID = _vendorRepo.Add(vendor, contact, address);

            var vendorResult = _vendorRepo.Find(vendorID);

            Assert.NotNull(vendorResult);
            Assert.Equal(vendor.AccountNumber, vendorResult.AccountNumber);

            var addressResult = _vendorRepo.GetVendorAddress(address.AddressID);
            Assert.NotNull(addressResult);
            Assert.Equal(address.AddressLine1, addressResult.AddressLine1);

            var contactResult = _vendorRepo.GetVendorContact(vendor.BusinessEntityID, contact.BusinessEntityID, vendor.BusinessEntityContacts[0].ContactTypeID);
            Assert.NotNull(contactResult);
            Assert.Equal(contact.EmailAddressObj.PersonEmailAddress, contactResult.EmailAddressObj.PersonEmailAddress);
        }

        [Fact]
        public void ShouldAddNewAddressToExistingVendor()
        {
            var vendorID = 5;
            var vendor = _vendorRepo.Find(vendorID);
            var addresses = _vendorRepo.GetVendorAddresses(vendor.BusinessEntityID);

            Assert.NotNull(addresses);
            int count = addresses.Count();
            Assert.Equal(1, count);

            var address = new Address
            {
                AddressLine1 = "1 Sunny Side Of The Road",
                City = "Berkeley",
                PostalCode = "94704",
                StateProvinceID = 9,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    BusinessEntityID = vendorID,
                    AddressTypeID = 7
                }
            };

            var addressID = _vendorRepo.AddVendorAddress(vendor.BusinessEntityID, address);
            addresses = _vendorRepo.GetVendorAddresses(vendor.BusinessEntityID);

            Assert.NotNull(addresses);
            count = addresses.Count();
            Assert.Equal(2, count);
        }
    }
}