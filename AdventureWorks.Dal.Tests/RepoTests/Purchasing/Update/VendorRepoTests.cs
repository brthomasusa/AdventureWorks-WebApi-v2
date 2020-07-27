using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Purchasing;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.Purchasing.Update
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
        public void ShouldUpdateVendor()
        {
            var vendorID = 5;
            var vendor = _vendorRepo.Find(v => v.BusinessEntityID == vendorID);

            Assert.NotNull(vendor);
            Assert.Equal("LIGHTSP0001", vendor.AccountNumber);
            Assert.Equal("Light Speed", vendor.Name);
            Assert.Equal(CreditRating.Superior, vendor.CreditRating);
            Assert.True(vendor.PreferredVendor);
            Assert.True(vendor.IsActive);

            vendor.AccountNumber = "LIGHTSP9999";
            vendor.Name = "Light Speed, Inc.";
            vendor.CreditRating = CreditRating.Average;
            vendor.PreferredVendor = false;
            vendor.IsActive = false;

            _vendorRepo.Update(vendor);

            var result = _vendorRepo.Find(v => v.BusinessEntityID == vendorID);
            Assert.NotNull(result);
            Assert.Equal("LIGHTSP9999", result.AccountNumber);
            Assert.Equal("Light Speed, Inc.", result.Name);
            Assert.Equal(CreditRating.Average, result.CreditRating);
            Assert.False(result.PreferredVendor);
            Assert.False(result.IsActive);
        }

        [Fact]
        public void ShouldRaiseExceptionWhileUpdatingDuplicateAccountNumber()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var vendorID = 5;
                var vendor = _vendorRepo.Find(v => v.BusinessEntityID == vendorID);
                vendor.AccountNumber = "CYCLERU0001";

                Action testCode = () =>
                {
                    _vendorRepo.Update(vendor);
                };

                var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
                Assert.Equal("Error: This operation would result in a duplicate vendor account number!", exception.Message);
            }
        }

        [Fact]
        public void ShouldUpdateVendorContact()
        {
            var personID = 11;
            var contact = _vendorRepo.GetVendorContact(personID);
            Assert.NotNull(contact);
            Assert.Equal("Marie", contact.FirstName);
            Assert.Equal("Moya", contact.LastName);

            contact.FirstName = "Monica";
            contact.LastName = "Smith";

            _vendorRepo.UpdateVendorContact(contact);

            var result = _vendorRepo.GetVendorContact(personID);
            Assert.NotNull(result);
            Assert.Equal("Monica", result.FirstName);
            Assert.Equal("Smith", result.LastName);
        }

        [Fact]
        public void ShouldUpdateAnExistingVendorAddress()
        {
            var addressID = 3;
            var address = _vendorRepo.GetVendorAddress(addressID);
            Assert.NotNull(address);
            Assert.Equal("298 Sunnybrook Drive", address.AddressLine1);
            Assert.Equal("Spring Valley", address.City);
            Assert.Equal("91977", address.PostalCode);

            address.AddressLine1 = "12354 Bad Luck Road";
            address.City = "Happy Village";
            address.PostalCode = "99999";

            _vendorRepo.UpdateVendorAddress(address);

            var result = _vendorRepo.GetVendorAddress(addressID);
            Assert.NotNull(result);
            Assert.Equal("12354 Bad Luck Road", result.AddressLine1);
            Assert.Equal("Happy Village", result.City);
            Assert.Equal("99999", result.PostalCode);
        }

        [Fact]
        public void ShouldRaiseExceptionUpdateVendorAddressWithDuplicate()
        {
            var addressID = 3;
            var address = _vendorRepo.GetVendorAddress(addressID);
            Assert.NotNull(address);

            address.AddressLine1 = "90 Sunny Ave";
            address.City = "Berkeley";
            address.PostalCode = "94704";

            Action testCode = () =>
            {
                _vendorRepo.UpdateVendorAddress(address);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: There is an existing entity with this address!", exception.Message);

        }
    }
}