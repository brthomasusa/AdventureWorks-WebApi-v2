using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;

namespace AdventureWorks.Dal.Tests.CrudTests.Person
{
    [Collection("AdventureWorks.Dal")]
    public class RetrieveTests : TestBase
    {
        [Fact]
        public void ShouldRetrievePersonWithPhoneEmailAndPassword()
        {
            SampleDataInitialization.InitializeData(ctx);

            var personObj = ctx.Person
                .AsNoTracking()
                .Where(person => person.PersonType == "EM" && person.FirstName == "Diane" && person.MiddleName == "L" && person.LastName == "Margheim")
                .Include(person => person.Phones)
                .Include(person => person.EmailAddressObj)
                .Include(Person => Person.PasswordObj)
                .Single<AdventureWorks.Models.Person.Person>();

            Assert.NotNull(personObj);
            Assert.Equal("815-555-0138", personObj.Phones[0].PhoneNumber);
            Assert.Equal("diane1@adventure-works.com", personObj.EmailAddressObj.PersonEmailAddress);
            Assert.Equal("s+FUWADIZzXBKpcbxe4OwL2uiJmjLogJNYXXHvc1X/k=", personObj.PasswordObj.PasswordHash);
        }

        [Fact]
        public void ShouldRetrieveOnePersonOfTypeEmployeeWithAddress()
        {
            SampleDataInitialization.InitializeData(ctx);

            var person = (
                from pTbl in ctx.Person
                join beaTbl in ctx.BusinessEntityAddress on pTbl.BusinessEntityID equals beaTbl.BusinessEntityID
                join aTbl in ctx.Address on beaTbl.AddressID equals aTbl.AddressID
                join sTbl in ctx.StateProvince on aTbl.StateProvinceID equals sTbl.StateProvinceID
                where pTbl.PersonType == "EM" && pTbl.BusinessEntityID == 1
                select new
                {
                    PersonID = pTbl.BusinessEntityID,
                    FirstName = pTbl.FirstName,
                    MiddleName = pTbl.MiddleName,
                    LastName = pTbl.LastName,
                    AddressLine1 = aTbl.AddressLine1,
                    AddressLine2 = aTbl.AddressLine2,
                    City = aTbl.City,
                    PostalCode = aTbl.PostalCode,
                    Region = sTbl.StateProvinceCode
                }
            ).Single();

            Assert.NotNull(person);
            Assert.Equal("Ken", person.FirstName);
            Assert.Equal("4350 Minute Dr.", person.AddressLine1);
        }

        [Fact]
        public void ShouldRetrieveAllPhoneViewModels()
        {
            SampleDataInitialization.InitializeData(ctx);

            var results = ctx.PhoneViewModel
                .OrderBy(r => r.BusinessEntityID)
                .ToList();

            Assert.NotNull(results);
            var count = results.Count;
            Assert.Equal(14, count);
        }

        [Fact]
        public void ShouldRetrieveOnePhoneViewModelByBusinessEntityID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var bizEntityID = 13;
            var result = ctx.PhoneViewModel
                .Where(ph => ph.BusinessEntityID == bizEntityID)
                .FirstOrDefault();


            Assert.NotNull(result);
            Assert.Equal("273-555-0100", result.PhoneNumber);
        }

        [Fact]
        public void ShouldRetrieveAllAddressViewModels()
        {
            SampleDataInitialization.InitializeData(ctx);

            var results = ctx.AddressViewModel
                .OrderBy(r => r.BusinessEntityID)
                .ThenBy(r => r.AddressID)
                .ToList();

            Assert.NotNull(results);
            var count = results.Count;
            Assert.Equal(11, count);
        }

        [Fact]
        public void ShouldRetrieveOneAddressViewModelByAddressID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var addressID = 1;
            var result = ctx.AddressViewModel
                .Where(a => a.AddressID == addressID)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal("28 San Marino Ct", result.AddressLine1);
        }        
    }
}