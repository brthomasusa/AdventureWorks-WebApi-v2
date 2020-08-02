using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Sales;
using AdventureWorks.Models.ViewModel;
using AdventureWorks.Dal.EfCode.Configuration.HumanResources;
using AdventureWorks.Dal.EfCode.Configuration.Person;
using AdventureWorks.Dal.EfCode.Configuration.Purchasing;
using AdventureWorks.Dal.EfCode.Configuration.Sales;

namespace AdventureWorks.Dal.EfCode
{
    public class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options) : base(options) { }

        public virtual DbSet<BusinessEntity> BusinessEntity { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<BusinessEntityAddress> BusinessEntityAddress { get; set; }
        public virtual DbSet<BusinessEntityContact> BusinessEntityContact { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<CountryRegion> CountryRegion { get; set; }
        public virtual DbSet<EmailAddress> EmailAddress { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonPhone> PersonPhone { get; set; }
        public virtual DbSet<PersonPWord> Password { get; set; }
        public virtual DbSet<PhoneNumberType> PhoneNumberType { get; set; }
        public virtual DbSet<StateProvince> StateProvince { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<SalesTaxRate> SalesTaxRate { get; set; }
        public virtual DbSet<SalesTerritory> SalesTerritory { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeDepartmentHistory> EmployeeDepartmentHistory { get; set; }
        public virtual DbSet<EmployeePayHistory> EmployeePayHistory { get; set; }
        public virtual DbSet<JobCandidate> JobCandidate { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }

        public virtual DbQuery<PhoneViewModel> PhoneViewModel { get; set; }
        public virtual DbQuery<AddressViewModel> AddressViewModel { get; set; }
        public virtual DbQuery<VendorContactViewModel> VendorContactViewModel { get; set; }
        public virtual DbQuery<VendorViewModel> VendorViewModel { get; set; }
        public virtual DbQuery<EmployeeViewModel> EmployeeViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Query<PhoneViewModel>().ToQuery(() => PhoneViewModel.FromSql(
              @"SELECT BusinessEntityID, PhoneNumber, ph.PhoneNumberTypeID, phtype.Name AS PhoneNumberType
                FROM Person.PersonPhone ph
                INNER JOIN Person.PhoneNumberType phtype ON ph.PhoneNumberTypeID = phtype.PhoneNumberTypeID"
            ).AsQueryable());

            modelBuilder.Query<PhoneViewModel>(query => query.Ignore(e => e.RowGuid));
            modelBuilder.Query<PhoneViewModel>(query => query.Ignore(e => e.ModifiedDate));

            modelBuilder.Query<AddressViewModel>().ToQuery(() => AddressViewModel.FromSql(
              @"SELECT bea.BusinessEntityID, a.AddressID, AddressLine1, AddressLine2, City, 
                a.StateProvinceID, StateProvinceCode, PostalCode, a.SpatialLocation,
                CountryRegionCode, bea.AddressTypeID, t.Name AS AddressTypeName
                FROM Person.Address a
                INNER JOIN Person.StateProvince st ON a.StateProvinceID = st.StateProvinceID
                INNER JOIN Person.BusinessEntityAddress bea ON a.AddressID = bea.AddressID
                INNER JOIN Person.AddressType t ON bea.AddressTypeID = t.AddressTypeID"
            ).AsQueryable());

            modelBuilder.Query<AddressViewModel>(query => query.Ignore(e => e.RowGuid));
            modelBuilder.Query<AddressViewModel>(query => query.Ignore(e => e.ModifiedDate));

            modelBuilder.Query<VendorContactViewModel>().ToQuery(() => VendorContactViewModel.FromSql(
              @"SELECT pp.BusinessEntityID, pp.PersonType, pp.NameStyle AS IsEasternNameStyle, pp.Title, pp.FirstName,
                    pp.MiddleName, pp.LastName, pp.Suffix, pp.EmailPromotion, pp.AdditionalContactInfo, 
                    pp.Demographics, email.EmailAddressID, email.EmailAddress, pw.PasswordHash AS EmailPasswordHash,
                    pw.PasswordSalt AS EmailPasswordSalt, bec.ContactTypeID, ct.Name AS ContactPosition, bec.BusinessEntityID AS VendorID
                FROM Person.Person pp
                INNER JOIN Person.EmailAddress email ON pp.BusinessEntityID = email.BusinessEntityID
                INNER JOIN Person.[Password] pw ON pp.BusinessEntityID = pw.BusinessEntityID
                INNER JOIN Person.BusinessEntityContact bec ON pp.BusinessEntityID = bec.PersonID
                INNER JOIN Person.ContactType ct ON bec.ContactTypeID = ct.ContactTypeID
                WHERE bec.BusinessEntityID IN (SELECT BusinessEntityID FROM Purchasing.Vendor)"
            ).AsQueryable());

            modelBuilder.Query<VendorContactViewModel>(query => query.Ignore(e => e.RowGuid));
            modelBuilder.Query<VendorContactViewModel>(query => query.Ignore(e => e.ModifiedDate));

            modelBuilder.Query<VendorViewModel>().ToQuery(() => VendorViewModel.FromSql(
              @"SELECT ven.BusinessEntityID, ven.AccountNumber, ven.Name,
                CAST(ven.CreditRating AS int) AS CreditRating, ven.PreferredVendorStatus AS PreferredVendor, 
                ven.PurchasingWebServiceURL, ven.ActiveFlag AS IsActive
                FROM Purchasing.Vendor ven"
            ).AsQueryable());

            modelBuilder.Query<VendorViewModel>(query => query.Ignore(e => e.RowGuid));
            modelBuilder.Query<VendorViewModel>(query => query.Ignore(e => e.ModifiedDate));

            modelBuilder.Query<EmployeeViewModel>().ToQuery(() => EmployeeViewModel.FromSql(
              @"SELECT pp.BusinessEntityID, pp.PersonType, pp.NameStyle AS IsEasternNameStyle, pp.Title, pp.FirstName,
                  pp.MiddleName, pp.LastName, pp.Suffix, pp.EmailPromotion, pp.AdditionalContactInfo, 
                  pp.Demographics, email.EmailAddressID, email.EmailAddress, pw.PasswordHash AS EmailPasswordHash,
                  pw.PasswordSalt AS EmailPasswordSalt, ee.NationalIDNumber, ee.LoginID, ee.JobTitle, ee.BirthDate,
                  ee.MaritalStatus, ee.Gender, ee.HireDate, ee.SalariedFlag AS IsSalaried, ee.VacationHours,
                  ee.SickLeaveHours, ee.CurrentFlag AS IsActive
              FROM Person.Person pp
              INNER JOIN Person.EmailAddress email ON pp.BusinessEntityID = email.BusinessEntityID
              INNER JOIN Person.[Password] pw ON pp.BusinessEntityID = pw.BusinessEntityID
              INNER JOIN HumanResources.Employee ee ON ee.BusinessEntityID = pp.BusinessEntityID"
            ).AsQueryable());

            modelBuilder.Query<EmployeeViewModel>(query => query.Ignore(e => e.RowGuid));
            modelBuilder.Query<EmployeeViewModel>(query => query.Ignore(e => e.ModifiedDate));

            modelBuilder.ApplyConfiguration(new BusinessEntityConfig());
            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new AddressTypeConfig());
            modelBuilder.ApplyConfiguration(new BusinessEntityAddressConfig());
            modelBuilder.ApplyConfiguration(new BusinessEntityContactConfig());
            modelBuilder.ApplyConfiguration(new ContactTypeConfig());
            modelBuilder.ApplyConfiguration(new CountryRegionConfig());
            modelBuilder.ApplyConfiguration(new EmailAddressConfig());
            modelBuilder.ApplyConfiguration(new PersonConfig());
            modelBuilder.ApplyConfiguration(new PersonPhoneConfig());
            modelBuilder.ApplyConfiguration(new PasswordConfig());
            modelBuilder.ApplyConfiguration(new PhoneNumberTypeConfig());
            modelBuilder.ApplyConfiguration(new StateProvinceConfig());
            modelBuilder.ApplyConfiguration(new VendorConfig());
            modelBuilder.ApplyConfiguration(new SalesTaxRateConfig());
            modelBuilder.ApplyConfiguration(new SalesTerritoryConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new EmployeeConfig());
            modelBuilder.ApplyConfiguration(new EmployeeDepartmentHistoryConfig());
            modelBuilder.ApplyConfiguration(new EmployeePayHistoryConfig());
            modelBuilder.ApplyConfiguration(new JobCandidateConfig());
            modelBuilder.ApplyConfiguration(new ShiftConfig());
        }

    }
}
