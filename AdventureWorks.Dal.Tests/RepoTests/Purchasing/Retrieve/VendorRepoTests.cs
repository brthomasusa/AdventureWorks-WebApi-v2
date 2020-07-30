using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using PersonClass = AdventureWorks.Models.Person.Person;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Retrieve
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
        public void ShouldGetVendorContactViewModels()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendorID = 7;
                var vendorContacts = _vendorRepo.GetVendorContactViewModels(vendorID);

                Assert.NotNull(vendorContacts);
                var count = vendorContacts.Count();
                Assert.Equal(3, count);
            }
        }

        [Fact]
        public void ShouldGetVendorAddressViewModelsForOneVendor()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendorID = 3;
                var vendorAddresses = _vendorRepo.GetVendorAddressViewModelsForOneVendor(vendorID);

                Assert.NotNull(vendorAddresses);
                int count = vendorAddresses.Count();
                Assert.Equal(2, count);
            }
        }

        [Fact]
        public void ShouldGetAllVendorRecords()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendors = _vendorRepo.GetAll();

                Assert.NotNull(vendors);
                var count = vendors.Count();
                Assert.Equal(6, count);
            }
        }

        [Fact]
        public void ShouldGetOneVendorByID()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendorID = 5;
                var vendor = _vendorRepo.Find(vendorID);

                Assert.NotNull(vendor);
                Assert.Equal("Light Speed", vendor.Name);
            }
        }

        [Fact]
        public void ShouldGetOneVendorByAccountNumber()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendor = _vendorRepo.Find(v => v.AccountNumber == "LIGHTSP0001");

                Assert.NotNull(vendor);
                Assert.Equal("Light Speed", vendor.Name);
            }
        }

        [Fact]
        public void ShouldGetOneVendorByName()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendor = _vendorRepo.Find(v => v.Name == "Light Speed");

                Assert.NotNull(vendor);
                Assert.Equal("LIGHTSP0001", vendor.AccountNumber);
            }
        }

        [Fact]
        public void ShouldGetVendorAddressByAddressID()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var addressID = 5;
                var address = _vendorRepo.GetVendorAddress(addressID);

                Assert.NotNull(address);
                Assert.Equal("211 East Pleasant Run Rd", address.AddressLine1);
                Assert.Equal("Desoto", address.City);
                Assert.Equal(73, address.StateProvinceID);
                Assert.Equal("75115", address.PostalCode);
            }
        }

        [Fact]
        public void ShouldGetAllAddressesForOneVendor()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendorID = 3;
                var addresses = _vendorRepo.GetVendorAddresses(vendorID);

                Assert.NotNull(addresses);
                int count = addresses.Count();
                Assert.Equal(2, count);
            }
        }

        [Fact]
        public void ShouldGetOneVendorContactByPersonID()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var personID = 8;
                var vendorContact = _vendorRepo.GetVendorContact(personID);

                Assert.NotNull(vendorContact);
                Assert.Equal("Johnny", vendorContact.FirstName);
                Assert.Equal("Dough", vendorContact.LastName);
            }
        }

        [Fact]
        public void ShouldGetOneContactForVendorUsingBizEntityContact()
        {
            PersonClass vendorContact = _vendorRepo.GetVendorContact(9);

            Assert.NotNull(vendorContact);
            Assert.Equal("Jo", vendorContact.FirstName);
            Assert.Equal("Zimmerman", vendorContact.LastName);
            Assert.Equal("816-555-0142", vendorContact.Phones[0].PhoneNumber);
        }











    }
}