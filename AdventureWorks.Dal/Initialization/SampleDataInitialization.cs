using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Sales;

namespace AdventureWorks.Dal.Initialization
{
    public static class SampleDataInitialization
    {
        internal static void ResetIdentity(AdventureWorksContext ctx)
        {
            var tables = new[]
            {
                "Person.BusinessEntity",
                "Person.Address",
                "Person.EmailAddress",
                "Sales.SalesTerritory",
                "HumanResources.JobCandidate",
                "HumanResources.Department",
                "HumanResources.Shift"
            };

            foreach (var table in tables)
            {
                var rawSqlString = $"DBCC CHECKIDENT (\"{table}\", RESEED, 0);";

#pragma warning disable EF1000  // Possible Sql injection vulnerability                
                ctx.Database.ExecuteSqlCommand(rawSqlString);
#pragma warning restore EF1000
            }
        }

        public static void ClearData(AdventureWorksContext ctx)
        {
            ctx.Database.ExecuteSqlCommand("DELETE FROM Sales.SalesTerritory");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.Password");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.PersonPhone");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.EmailAddress");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.BusinessEntityAddress");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.BusinessEntityContact");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.Address");

            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.EmployeePayHistory");
            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.EmployeeDepartmentHistory");
            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.JobCandidate");
            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.Shift");
            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.Department");

            // Trigger HumanResources.dEmployee prevents deletion of an employee, only allows active flag to be set
            ctx.Database.ExecuteSqlCommand("DISABLE TRIGGER HumanResources.dEmployee ON HumanResources.Employee;");
            ctx.Database.ExecuteSqlCommand("DELETE FROM HumanResources.Employee");
            ctx.Database.ExecuteSqlCommand("ENABLE TRIGGER HumanResources.dEmployee ON HumanResources.Employee;");

            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.Person");

            // Trigger Purchasing.dVendor prevents deletion of a vendor, only allows active flag to be set
            ctx.Database.ExecuteSqlCommand("DISABLE TRIGGER Purchasing.dVendor ON Purchasing.Vendor;");
            ctx.Database.ExecuteSqlCommand("DELETE FROM Purchasing.Vendor");
            ctx.Database.ExecuteSqlCommand("ENABLE TRIGGER Purchasing.dVendor ON Purchasing.Vendor;");

            ctx.Database.ExecuteSqlCommand("DELETE FROM Person.BusinessEntity");

            ResetIdentity(ctx);
        }

        internal static void SeedData(AdventureWorksContext ctx)
        {
            try
            {
                if (!ctx.BusinessEntity.Any())
                {
                    ctx.Shift.AddRange(SampleData.GetHumanResourceShifts());
                    ctx.Department.AddRange(SampleData.GetHRDepartments());
                    ctx.SaveChanges();

                    ctx.BusinessEntity.AddRange(SampleData.GetBusinessEntities());
                    ctx.SalesTerritory.AddRange(SampleData.GetSalesTerritories());
                    ctx.SaveChanges();

                    AddVendorAddressesToDb(ctx);
                    AddPeopleAddressesToDb(ctx);
                    AddEmployeeDeptHistoryToDb(ctx);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void InitializeData(AdventureWorksContext ctx)
        {
            ClearData(ctx);
            SeedData(ctx);
        }

        public static void AddPeopleToDb(AdventureWorksContext ctx)
        {
            BusinessEntity businessEntity1 = new BusinessEntity { };
            BusinessEntity businessEntity2 = new BusinessEntity { };
            ctx.BusinessEntity.AddRange(new List<BusinessEntity> { businessEntity1, businessEntity2 });

            var PersonObj1 = new Person
            {
                BusinessEntityID = businessEntity1.BusinessEntityID,
                PersonType = "EM",
                IsEasternNameStyle = false,
                FirstName = "Ken",
                MiddleName = "J",
                LastName = "Sanchez",
                EmailPromotion = EmailPromoPreference.NoPromotions,
                EmailAddressObj = new EmailAddress
                {
                    PersonEmailAddress = "ken@adventure-works.com"
                },
                Phones = 
                {
                    new PersonPhone {PhoneNumber = "697-555-0142", PhoneNumberTypeID = 1}                    
                },
                PasswordObj = new PersonPWord
                {
                    PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                    PasswordSalt = "bE3XiWw="
                }
            };

            var PersonObj2 = new Person
            {
                BusinessEntityID = businessEntity2.BusinessEntityID,
                PersonType = "EM",
                IsEasternNameStyle = false,
                FirstName = "Terri",
                MiddleName = "Lee",
                LastName = "Duffy",
                EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                EmailAddressObj = new EmailAddress
                {
                    PersonEmailAddress = "terri@adventure-works.com"
                },
                Phones = 
                {
                    new PersonPhone {PhoneNumber = "819-555-0175", PhoneNumberTypeID = 3}                    
                },
                PasswordObj = new PersonPWord
                {
                    PasswordHash = "bawRVNrZQYQ05qF05Gz6VLilnviZmrqBReTTAGAudm0=",
                    PasswordSalt = "EjJaC3U="
                }
            };

            ctx.Person.AddRange(new List<Person> { PersonObj1, PersonObj2 });
            ctx.SaveChanges();
        }

        private static void AddPeopleAddressesToDb(AdventureWorksContext ctx)
        {
            if (ctx.Person.Any())
            {
                var peopleAddresses = SampleData.GetPeopleAddresses();

                var query = ctx.Person.AsNoTracking()
                    .Where(p => p.FirstName == "Ken" && p.MiddleName == "J" && p.LastName == "Sanchez")
                    .Select(obj => new { obj.BusinessEntityID })
                    .SingleOrDefault();

                var address = peopleAddresses.FirstOrDefault(a => a.AddressLine1 == "4350 Minute Dr." && a.City == "Newport Hills");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = query.BusinessEntityID;
                    ctx.Address.Add(address);
                }

                query = ctx.Person.AsNoTracking()
                    .Where(p => p.FirstName == "Terri" && p.MiddleName == "Lee" && p.LastName == "Duffy")
                    .Select(v => new { v.BusinessEntityID })
                    .SingleOrDefault();

                address = peopleAddresses.FirstOrDefault(a => a.AddressLine1 == "7559 Worth Ct." && a.City == "Renton");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = query.BusinessEntityID;
                    ctx.Address.Add(address);
                }

                query = ctx.Person.AsNoTracking()
                    .Where(p => p.FirstName == "Gail" && p.MiddleName == "A" && p.LastName == "Erickson")
                    .Select(v => new { v.BusinessEntityID })
                    .SingleOrDefault();

                address = peopleAddresses.FirstOrDefault(a => a.AddressLine1 == "9435 Breck Court" && a.City == "Bellevue");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = query.BusinessEntityID;
                    ctx.Address.Add(address);
                }

                query = ctx.Person.AsNoTracking()
                    .Where(p => p.FirstName == "Jossef" && p.MiddleName == "H" && p.LastName == "Goldberg")
                    .Select(v => new { v.BusinessEntityID })
                    .SingleOrDefault();

                address = peopleAddresses.FirstOrDefault(a => a.AddressLine1 == "5670 Bel Air Dr." && a.City == "Renton");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = query.BusinessEntityID;
                    ctx.Address.Add(address);
                }

                query = ctx.Person.AsNoTracking()
                    .Where(p => p.FirstName == "Diane" && p.MiddleName == "L" && p.LastName == "Margheim")
                    .Select(v => new { v.BusinessEntityID })
                    .SingleOrDefault();

                address = peopleAddresses.FirstOrDefault(a => a.AddressLine1 == "475 Santa Maria" && a.City == "Everett");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = query.BusinessEntityID;
                    ctx.Address.Add(address);
                }

                ctx.SaveChanges();
            }
        }

        private static void AddEmployeeDeptHistoryToDb(AdventureWorksContext ctx)
        {
            if (ctx.Employee.Any())
            {
                // Get department and shift ID's
                var engineering = ctx.Department
                    .AsNoTracking()
                    .Where(d => d.Name == "Engineering")
                    .Select(obj => new { deptID = obj.DepartmentID })
                    .SingleOrDefault();

                var RD = ctx.Department
                    .AsNoTracking()
                    .Where(d => d.Name == "Research and Development")
                    .Select(obj => new { deptID = obj.DepartmentID })
                    .SingleOrDefault();

                var executive = ctx.Department
                    .AsNoTracking()
                    .Where(d => d.Name == "Executive")
                    .Select(obj => new { deptID = obj.DepartmentID })
                    .SingleOrDefault();

                var shift = ctx.Shift
                    .AsNoTracking()
                    .Where(s => s.Name == "Day")
                    .Select(obj => new { ID = obj.ShiftID })
                    .SingleOrDefault();

                // For each employee (there are five), get the BusinessEntityID 
                // and create a EmployeeDepartmentHistory record
                var emp1 = ctx.Person
                    .AsNoTracking()
                    .Where(ee => ee.FirstName == "Ken" && ee.LastName == "Sanchez")
                    .Select(obj => new { ID = obj.BusinessEntityID })
                    .SingleOrDefault();

                var deptHistory1 = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp1.ID,
                    DepartmentID = executive.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2009, 1, 14)
                };

                var emp2 = ctx.Person
                    .AsNoTracking()
                    .Where(ee => ee.FirstName == "Terri" && ee.LastName == "Duffy")
                    .Select(obj => new { ID = obj.BusinessEntityID })
                    .SingleOrDefault();

                var deptHistory2 = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp2.ID,
                    DepartmentID = engineering.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2008, 1, 31)
                };

                var emp3 = ctx.Person
                    .AsNoTracking()
                    .Where(ee => ee.FirstName == "Gail" && ee.LastName == "Erickson")
                    .Select(obj => new { ID = obj.BusinessEntityID })
                    .SingleOrDefault();

                var deptHistory3 = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp3.ID,
                    DepartmentID = executive.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2008, 1, 6)
                };

                var emp4 = ctx.Person
                    .AsNoTracking()
                    .Where(ee => ee.FirstName == "Jossef" && ee.LastName == "Goldberg")
                    .Select(obj => new { ID = obj.BusinessEntityID })
                    .SingleOrDefault();

                var deptHistory4 = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp4.ID,
                    DepartmentID = executive.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2008, 1, 24)
                };

                var emp5 = ctx.Person
                    .AsNoTracking()
                    .Where(ee => ee.FirstName == "Diane" && ee.LastName == "Margheim")
                    .Select(obj => new { ID = obj.BusinessEntityID })
                    .SingleOrDefault();

                var deptHistory5 = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp5.ID,
                    DepartmentID = RD.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2008, 12, 29)
                };

                // Add new EmployeeDepartmentHistory entities to the database
                ctx.EmployeeDepartmentHistory.AddRange(new List<EmployeeDepartmentHistory> { deptHistory1, deptHistory2, deptHistory3, deptHistory4, deptHistory5 });

                ctx.SaveChanges();
            }
        }


        private static void AddVendorAddressesToDb(AdventureWorksContext ctx)
        {
            if (ctx.Vendor.Any())
            {
                var vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "AUSTRALI0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var vendorAddresses = SampleData.GetVendorAddresses();
                var address = vendorAddresses.FirstOrDefault(a => a.AddressLine1 == "28 San Marino Ct" && a.City == "Bellingham");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = vendor.ID;
                    ctx.Address.Add(address);
                }

                var contact1 = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Paula" && c.MiddleName == "B." && c.LastName == "Moberly")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();
                var contact2 = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Charlene" && c.MiddleName == "J." && c.LastName == "Wojcik")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();
                var contact3 = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Jo" && c.MiddleName == "J." && c.LastName == "Zimmerman")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var bc1 = new BusinessEntityContact { BusinessEntityID = vendor.ID, PersonID = contact1.ID, ContactTypeID = 18 };
                var bc2 = new BusinessEntityContact { BusinessEntityID = vendor.ID, PersonID = contact2.ID, ContactTypeID = 19 };
                var bc3 = new BusinessEntityContact { BusinessEntityID = vendor.ID, PersonID = contact3.ID, ContactTypeID = 17 };

                ctx.BusinessEntityContact.AddRange(new List<BusinessEntityContact> { bc1, bc2, bc3 });


                vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "TRIKES0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                address = vendorAddresses.FirstOrDefault(a => a.AddressLine1 == "90 Sunny Ave" && a.City == "Berkeley");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = vendor.ID;
                    ctx.Address.Add(address);
                }

                var contact4 = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "William" && c.MiddleName == "J." && c.LastName == "Monroe")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var bc4 = new BusinessEntityContact { BusinessEntityID = vendor.ID, PersonID = contact4.ID, ContactTypeID = 18 };

                ctx.BusinessEntityContact.Add(bc4);


                vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "LIGHTSP0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                address = vendorAddresses.FirstOrDefault(a => a.AddressLine1 == "298 Sunnybrook Drive" && a.City == "Spring Valley");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = vendor.ID;
                    ctx.Address.Add(address);
                }

                var contact5 = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Marie" && c.MiddleName == "E." && c.LastName == "Moya")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var bc5 = new BusinessEntityContact { BusinessEntityID = vendor.ID, PersonID = contact5.ID, ContactTypeID = 17 };

                ctx.BusinessEntityContact.Add(bc5);

                ctx.SaveChanges();
            }
        }
    }
}