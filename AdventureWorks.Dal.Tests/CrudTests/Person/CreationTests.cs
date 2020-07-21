using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.Tests.CrudTests.Person
{
    [Collection("AdventureWorks.Dal")]
    public class CreationTests : TestBase
    {
        [Fact]
        public void ShouldAddNewBusinessEntityRecord()
        {
            SampleDataInitialization.InitializeData(ctx);
            Assert.Equal(19, ctx.BusinessEntity.Count());

            var businessEntity = new BusinessEntity { };
            ctx.BusinessEntity.Add(businessEntity);
            ctx.SaveChanges();

            Assert.True(businessEntity.BusinessEntityID > 0);
            Assert.Equal(20, ctx.BusinessEntity.Count());
        }

        [Fact]
        public void ShouldAddNewPersonRecord()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var numberOfEntities = ctx.BusinessEntity.Count();
                var numberOfPeople = ctx.Person.Count();

                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        Title = "Big Pimp",
                        FirstName = "Terri",
                        MiddleName = "J",
                        LastName = "Phide",
                        Suffix = "Mr.",
                        EmailPromotion = EmailPromoPreference.NoPromotions
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                Assert.Equal(numberOfEntities + 1, ctx.BusinessEntity.Count());
                Assert.Equal(numberOfPeople + 1, ctx.Person.Count());
                Assert.Equal(businessEntity.BusinessEntityID, businessEntity.PersonObj.BusinessEntityID);
            }
        }

        [Fact]
        public void ShouldAddNewPersonWithNewEmailAddressAndEmailPassword()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        EmailAddressObj = new EmailAddress
                        {
                            PersonEmailAddress = "jdoe@adventure-works.com"
                        },
                        PasswordObj = new PersonPWord
                        {
                            PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                            PasswordSalt = "bE3XiWw="
                        }
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);
                Assert.True(person.BusinessEntityID > 0);

                var emailAddress = ctx.EmailAddress
                    .AsNoTracking()
                    .Where(a => a.BusinessEntityID == person.BusinessEntityID)
                    .SingleOrDefault<EmailAddress>();

                Assert.Equal("jdoe@adventure-works.com", emailAddress.PersonEmailAddress);

                var emailPassword = ctx.Password
                    .AsNoTracking()
                    .Where(pword => pword.BusinessEntityID == person.BusinessEntityID)
                    .SingleOrDefault<PersonPWord>();

                Assert.Equal("pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=", emailPassword.PasswordHash);
            }
        }

        [Fact]
        public void ShouldAddPersonWithNewPhoneNumber()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        Phones = 
                        {
                            new PersonPhone {PhoneNumber = "888-555-0142", PhoneNumberTypeID = 1}                            
                        }
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);
                Assert.True(person.BusinessEntityID > 0);

                var phoneNumber = ctx.PersonPhone
                    .AsNoTracking()
                    .Where(phone => phone.BusinessEntityID == person.BusinessEntityID)
                    .SingleOrDefault<PersonPhone>();

                Assert.Equal("888-555-0142", phoneNumber.PhoneNumber);
            }
        }

        [Fact]
        public void ShouldAddPersonWithNewBusinessEntityAddress()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var address = new Address
                {
                    AddressLine1 = "123 Main Street",
                    AddressLine2 = "Ste 111",
                    City = "Dallas",
                    StateProvinceID = 73,
                    PostalCode = "75231",
                    BusinessEntityAddressObj = new BusinessEntityAddress
                    {
                        BusinessEntityID = businessEntity.BusinessEntityID,
                        AddressTypeID = 3
                    }
                };

                ctx.Address.Add(address);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);
                Assert.True(person.BusinessEntityID > 0);

                var businessEntityAddress = ctx.BusinessEntityAddress
                    .AsNoTracking()
                    .Where(bea => bea.BusinessEntityID == person.BusinessEntityID && bea.AddressID == address.AddressID)
                    .Single<BusinessEntityAddress>();

                Assert.NotNull(businessEntityAddress);
            }
        }
    }
}