using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;

namespace AdventureWorks.Dal.Tests.CrudTests.Purchasing
{
    [Collection("AdventureWorks.Dal")]
    public class CreationTests : TestBase
    {
        [Fact]
        public void ShouldCreateVendorRecord()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    VendorObj = new Vendor
                    {
                        AccountNumber = "OAKCLIFF0001",
                        Name = "Oak Cliff Bike Resellers",
                        CreditRating = CreditRating.Superior,
                        PreferredVendor = true,
                        IsActive = true
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var vendor = ctx.Vendor
                    .AsNoTracking()
                    .Where(v => v.AccountNumber == "OAKCLIFF0001")
                    .Single<Vendor>();

                Assert.NotNull(vendor);
            }
        }

        [Fact]
        public void ShouldAddVendorAndContactThenLinkContactToVendor()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                // Create the vendor
                var vendorEntity = new BusinessEntity
                {
                    VendorObj = new Vendor
                    {
                        AccountNumber = "OAKCLIFF0001",
                        Name = "Oak Cliff Bike Resellers",
                        CreditRating = CreditRating.Superior,
                        PreferredVendor = true,
                        IsActive = true
                    }
                };

                // Create the vendor point of contact with phone, email, and email password
                var contactEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "VC",
                        IsEasternNameStyle = false,
                        Title = "Big Pimp",
                        FirstName = "Terri",
                        MiddleName = "J",
                        LastName = "Phide",
                        Suffix = "Mr.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        EmailAddressObj = new EmailAddress
                        {
                            PersonEmailAddress = "terri.phide@adventure-works.com"
                        },
                        PasswordObj = new PersonPWord
                        {
                            PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                            PasswordSalt = "bE3XiWw="
                        },
                        Phones = 
                        {
                            new PersonPhone {PhoneNumber = "214-555-0142", PhoneNumberTypeID = 1}                            
                        }
                    }
                };

                // Insert vendor and point of contact into the database
                ctx.BusinessEntity.AddRange(new List<BusinessEntity> { vendorEntity, contactEntity });
                ctx.SaveChanges();

                // Create the vendor's address
                var address = new Address
                {
                    AddressLine1 = "123 MLK Blvd",
                    AddressLine2 = "Ste 111",
                    City = "Dallas",
                    StateProvinceID = 73,
                    PostalCode = "75231",
                    BusinessEntityAddressObj = new BusinessEntityAddress
                    {
                        BusinessEntityID = vendorEntity.BusinessEntityID,
                        AddressTypeID = 3
                    }
                };

                // Create the link between the vendor's address and the vendor
                var vendorContactLink = new BusinessEntityContact
                {
                    BusinessEntityID = vendorEntity.BusinessEntityID,
                    PersonID = contactEntity.BusinessEntityID,
                    ContactTypeID = 17
                };

                // Insert the vendor address and address linking record
                ctx.Address.Add(address);
                ctx.BusinessEntityContact.Add(vendorContactLink);
                ctx.SaveChanges();

                /*
                    Adding a vendor and vendor contact results in records being inserted into
                    the following eight tables:
                        Purchasing.Vendor
                        Person.Person
                        Person.PersonPhone
                        Person.EmailAddress
                        Person.Password
                        Person.Address
                        Person.BusinessEntityAddress
                        Person.BusinessEntityContact
                */
                var vendor = ctx.Vendor
                    .AsNoTracking()
                    .Where(v => v.AccountNumber == "OAKCLIFF0001")
                    .Single<Vendor>();

                var contact = ctx.Person
                    .AsNoTracking()
                    .Where(c => c.FirstName == "Terri" && c.MiddleName == "J" && c.LastName == "Phide")
                    .Single<AdventureWorks.Models.Person.Person>();

                var contactLink = ctx.BusinessEntityContact
                    .AsNoTracking()
                    .Where(bec => bec.BusinessEntityID == vendor.BusinessEntityID && bec.PersonID == contact.BusinessEntityID)
                    .Single<BusinessEntityContact>();

                var addressLink = ctx.BusinessEntityAddress
                    .AsNoTracking()
                    .Where(a => a.BusinessEntityID == vendor.BusinessEntityID && a.AddressID == address.AddressID && a.AddressTypeID == 3)
                    .Single<BusinessEntityAddress>();

                var vendorAddress = ctx.Address
                    .AsNoTracking()
                    .Where(a => a.AddressLine1 == "123 MLK Blvd" && a.AddressLine2 == "Ste 111" && a.City == "Dallas" && a.StateProvinceID == 73)
                    .Single<Address>();

                var vendorContactEmail = ctx.EmailAddress
                    .AsNoTracking()
                    .Where(e => e.BusinessEntityID == contactEntity.BusinessEntityID && e.PersonEmailAddress == "terri.phide@adventure-works.com")
                    .Single<EmailAddress>();

                var vendorContactPhone = ctx.PersonPhone
                    .AsNoTracking()
                    .Where(ph => ph.BusinessEntityID == contactEntity.BusinessEntityID && ph.PhoneNumber == "214-555-0142" && ph.PhoneNumberTypeID == 1)
                    .Single<PersonPhone>();

                var contactPassword = ctx.Password
                    .AsNoTracking()
                    .Where(pw => pw.BusinessEntityID == contactEntity.BusinessEntityID && pw.PasswordHash == "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=")
                    .Single<PersonPWord>();

                Assert.NotNull(vendor);
                Assert.NotNull(contact);
                Assert.NotNull(contactLink);
                Assert.NotNull(addressLink);
                Assert.NotNull(vendorAddress);
                Assert.NotNull(vendorContactEmail);
                Assert.NotNull(vendorContactPhone);
                Assert.NotNull(contactPassword);
            }
        }
    }
}