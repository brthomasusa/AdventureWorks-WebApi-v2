using System.Linq;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.InitializationTests
{
    [Collection("AdventureWorks.Dal")]
    public class InitializationTests : TestBase
    {
        [Fact]
        public void ShouldLoadShiftRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            Assert.Equal(3, ctx.Shift.Count());
        }

        [Fact]
        public void ShouldLoadDepartmentRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            Assert.Equal(16, ctx.Department.Count());
        }

        [Fact]
        public void ShouldLoadEmployeeRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var employees = ctx.Employee.ToList();
            Assert.Equal(6, employees.Count());
        }

        [Fact]
        public void ShouldLoadEmployeePayHistoryRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var payHistories = ctx.EmployeePayHistory.ToList();
            Assert.Equal(7, payHistories.Count());
        }

        [Fact]
        public void ShouldLoadEmployeeDepartmentHistoryRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var deptHistories = ctx.EmployeeDepartmentHistory.ToList();
            Assert.Equal(6, deptHistories.Count());
        }

        [Fact]
        public void ShouldLoadBusinessEntityRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var businessEntityList = ctx.BusinessEntity.ToList();

            Assert.Equal(19, businessEntityList.Count());

            var businessEntity = ctx.BusinessEntity.Single(be => be.BusinessEntityID == 1);
            Assert.NotNull(businessEntity);

            Assert.NotNull(businessEntity.PersonObj);
            Assert.Equal("Sanchez", businessEntity.PersonObj.LastName);

            Assert.NotNull(businessEntity.PersonObj.EmployeeObj.LoginID);
            Assert.Equal("adventure-works\\ken0", businessEntity.PersonObj.EmployeeObj.LoginID);
        }

        [Fact]
        public void ShouldLoadPersonRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var people = ctx.Person.ToList();
            Assert.Equal(13, people.Count());
        }

        [Fact]
        public void ShouldLoadVendorRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var vendors = ctx.Vendor.ToList();
            Assert.Equal(6, vendors.Count());
        }

        [Fact]
        public void ShouldLoadAddressRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var addresses = ctx.Address.ToList();
            Assert.Equal(11, addresses.Count());
        }

        [Fact]
        public void ShouldLoadAddressTypeRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var addressTypes = ctx.AddressType.ToList();
            Assert.Equal(7, addressTypes.Count());
        }

        [Fact]
        public void ShouldLoadBusinessEntityAddressRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var addresses = ctx.BusinessEntityAddress.ToList();
            Assert.Equal(11, addresses.Count());
        }

        [Fact]
        public void ShouldLoadBusinessEntityContactRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var contacts = ctx.BusinessEntityContact.ToList();
            Assert.Equal(7, contacts.Count());
        }

        [Fact]
        public void ShouldLoadContactTypeRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var contactTypes = ctx.ContactType.ToList();
            Assert.Equal(20, contactTypes.Count());
        }

        [Fact]
        public void ShouldLoadCountryRegionRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var regions = ctx.CountryRegion.ToList();
            Assert.Equal(238, regions.Count());
        }

        [Fact]
        public void ShouldLoadEmailAddressRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var emailAddresses = ctx.EmailAddress.ToList();
            Assert.Equal(13, emailAddresses.Count());
        }

        [Fact]
        public void ShouldLoadPasswordRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var passwords = ctx.Password.ToList();
            Assert.Equal(13, passwords.Count());
        }

        [Fact]
        public void ShouldLoadPersonPhoneRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var phones = ctx.PersonPhone.ToList();
            Assert.Equal(14, phones.Count());
        }

        [Fact]
        public void ShouldLoadPhoneNumberTypeRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var phoneTypes = ctx.PhoneNumberType.ToList();
            Assert.Equal(3, phoneTypes.Count());
        }

        [Fact]
        public void ShouldLoadStateProvinceRecords()
        {
            SampleDataInitialization.InitializeData(ctx);
            var states = ctx.StateProvince.ToList();
            Assert.Equal(181, states.Count());
        }

        // [Fact]
        // public void ShouldLoadEmployeesFromView()
        // {
        //     SampleDataInitialization.InitializeData(ctx);
        //     var employees = ctx.PersonEmployee.ToList();
        //     Assert.Equal(5, employees.Count());
        // }

        // [Fact]
        // public void ShouldLoadVendorContactsFromView()
        // {
        //     SampleDataInitialization.InitializeData(ctx);
        //     var vendorContacts = ctx.VendorContact.ToList();
        //     Assert.Equal(5, vendorContacts.Count());
        // }
    }
}