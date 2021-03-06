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

                    AddAddressesAndContactsToVendor(ctx);
                    AddAddressesToEmployees(ctx);
                    AddEmployeeDeptHistoryToDb(ctx);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*** SeedData() Error: {ex}");
            }
        }

        public static void InitializeData(AdventureWorksContext ctx)
        {
            ClearData(ctx);
            SeedData(ctx);
        }

        private static void AddAddressesToEmployees(AdventureWorksContext ctx)
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

                var toolDesign = ctx.Department
                    .AsNoTracking()
                    .Where(d => d.Name == "Tool Design")
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
                    DepartmentID = toolDesign.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2008, 1, 31)
                };

                var deptHistory2A = new EmployeeDepartmentHistory
                {
                    BusinessEntityID = emp2.ID,
                    DepartmentID = engineering.deptID,
                    ShiftID = shift.ID,
                    StartDate = new DateTime(2010, 11, 3)
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
                ctx.EmployeeDepartmentHistory.AddRange(new List<EmployeeDepartmentHistory> { deptHistory1, deptHistory2, deptHistory2A, deptHistory3, deptHistory4, deptHistory5 });

                ctx.SaveChanges();
            }
        }

        private static void AddAddressesAndContactsToVendor(AdventureWorksContext ctx)
        {
            if (ctx.Vendor.Any())
            {
                var vendorAddresses = SampleData.GetVendorAddresses();

                // AUSTRALI0001
                var vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "AUSTRALI0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var address = vendorAddresses.First(a => a.AddressLine1 == "28 San Marino Ct" && a.City == "Bellingham");
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


                // TRIKES0001
                vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "TRIKES0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                address = vendorAddresses.First(a => a.AddressLine1 == "90 Sunny Ave" && a.City == "Berkeley");
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


                // LIGHTSP0001
                vendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "LIGHTSP0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                address = vendorAddresses.First(a => a.AddressLine1 == "298 Sunnybrook Drive" && a.City == "Spring Valley");
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

                // DFWBIRE0001
                var dfwVendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "DFWBIRE0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var terriPhide = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Terri" && c.LastName == "Phide")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var bc6 = new BusinessEntityContact { BusinessEntityID = dfwVendor.ID, PersonID = terriPhide.ID, ContactTypeID = 17 };
                ctx.BusinessEntityContact.Add(bc6);


                // DESOTOB0001
                var desotoVendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "DESOTOB0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var address1 = vendorAddresses.First(a => a.AddressLine1 == "1900 Desoto Court" && a.City == "Desoto");
                if (address1 != null)
                {
                    address1.BusinessEntityAddressObj.BusinessEntityID = desotoVendor.ID;
                    address1.BusinessEntityAddressObj.AddressTypeID = 3;
                    ctx.Address.Add(address1);
                }

                var address2 = vendorAddresses.FirstOrDefault(a => a.AddressLine1 == "211 East Pleasant Run Rd" && a.City == "Desoto");
                if (address2 != null)
                {
                    address2.BusinessEntityAddressObj.BusinessEntityID = desotoVendor.ID;
                    address2.BusinessEntityAddressObj.AddressTypeID = 7;
                    ctx.Address.Add(address2);
                }

                var johnnyDough = ctx.Person.AsNoTracking()
                    .Where(c => c.FirstName == "Johnny" && c.LastName == "Dough")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                var bc7 = new BusinessEntityContact { BusinessEntityID = desotoVendor.ID, PersonID = johnnyDough.ID, ContactTypeID = 17 };
                ctx.BusinessEntityContact.Add(bc7);


                // CYCLERU0001
                var rusVendor = ctx.Vendor.AsNoTracking()
                    .Where(v => v.AccountNumber == "CYCLERU0001")
                    .Select(c => new { ID = c.BusinessEntityID })
                    .SingleOrDefault();

                address = vendorAddresses.FirstOrDefault(a => a.AddressLine1 == "6266 Melody Lane" && a.City == "Dallas");
                if (address != null)
                {
                    address.BusinessEntityAddressObj.BusinessEntityID = rusVendor.ID;
                    address.BusinessEntityAddressObj.AddressTypeID = 3;
                    ctx.Address.Add(address);
                }


                ctx.SaveChanges();
            }
        }
    }
}