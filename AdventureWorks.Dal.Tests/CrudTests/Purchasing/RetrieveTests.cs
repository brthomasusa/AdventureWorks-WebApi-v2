using System.Collections.Generic;
using System.Linq;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using AdventureWorks.Models.ViewModel;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Dal.Tests.CrudTests.Purchasing
{
    [Collection("AdventureWorks.Dal")]
    public class RetrieveTests : TestBase
    {
        [Fact]
        public void ShouldRetrieveAllVendorsWithRelatedAddresses()
        {
            SampleDataInitialization.InitializeData(ctx);

            var vendors = (
                from vTbl in ctx.Vendor
                join beaTbl in ctx.BusinessEntityAddress on vTbl.BusinessEntityID equals beaTbl.BusinessEntityID
                join aTbl in ctx.Address on beaTbl.AddressID equals aTbl.AddressID
                join sTbl in ctx.StateProvince on aTbl.StateProvinceID equals sTbl.StateProvinceID
                orderby vTbl.Name
                select new
                {
                    VendorID = vTbl.BusinessEntityID,
                    AccountNumber = vTbl.AccountNumber,
                    VendorName = vTbl.Name,
                    AddressLine1 = aTbl.AddressLine1,
                    AddressLine2 = aTbl.AddressLine2,
                    City = aTbl.City,
                    PostalCode = aTbl.PostalCode,
                    Region = sTbl.StateProvinceCode
                }
            ).ToList();

            int vendorsCount = vendors.Count();

            Assert.NotNull(vendors);
            Assert.Equal(6, vendorsCount);
        }

        [Fact]
        public void ShouldRetrieveOneVendorContactFromView()
        {
            SampleDataInitialization.InitializeData(ctx);

            var vendorID = 5;

            var vendorContact = ctx.VendorContact
                .Where(vc => vc.BusinessEntityID == vendorID)
                .First<VendorContact>();

            Assert.Equal("Light Speed", vendorContact.Name);
        }

        [Fact]
        public void ShouldRetrieveAllVendorContactViewModels()
        {
            SampleDataInitialization.InitializeData(ctx);

            var results = ctx.VendorContactViewModel
                .OrderBy(r => r.BusinessEntityID)
                .ToList();

            Assert.NotNull(results);
            var count = results.Count;
            Assert.Equal(7, count);            
        }

        [Fact]
        public void ShouldRetrieveAllVendorContactViewModelsByVendorID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var vendorID = 7;

            var results = ctx.VendorContactViewModel
                .Where(vc => vc.VendorID == vendorID)
                .OrderBy(vc => vc.LastName)
                .ThenBy(vc => vc.FirstName)
                .ToList();

            Assert.NotNull(results);
            var count = results.Count;
            Assert.Equal(3, count);
            Assert.Equal("Moberly", results[0].LastName);            
            Assert.Equal("Zimmerman", results[2].LastName); 
        }

        [Fact]
        public void ShouldRetrieveOneVendorContactViewModel()
        {
            SampleDataInitialization.InitializeData(ctx);

            var bizEntityID = 13;
            var result = ctx.VendorContactViewModel
                .Where(r => r.BusinessEntityID == bizEntityID)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal("paula2@adventure-works.com", result.EmailAddress);
            Assert.Equal("VC", result.PersonType);
            Assert.Equal("Vendor Contact", result.PersonTypeLong);          
        }

        [Fact]
        public void ShouldRetrieveAllVendorViewModels()
        {
            SampleDataInitialization.InitializeData(ctx);

            var results = ctx.VendorViewModel
                .OrderBy(r => r.BusinessEntityID)
                .ToList();

            Assert.NotNull(results);
            var count = results.Count;
            Assert.Equal(6, count);             
        }

        [Fact]
        public void ShouldRetrieveOneVendorViewModel()
        {
            SampleDataInitialization.InitializeData(ctx);

            var bizEntityID = 2;
            var result = ctx.VendorViewModel
                .Where(v => v.BusinessEntityID == bizEntityID)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(CreditRating.Superior, result.CreditRating);
            Assert.Equal("Superior", result.CreditRatingText);
        }            
    }
}