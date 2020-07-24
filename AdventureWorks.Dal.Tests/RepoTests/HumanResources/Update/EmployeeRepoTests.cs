using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeRepoTests : RepoTestsBase
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeRepoTests()
        {
            _employeeRepo = new EmployeeRepo(ctx);
        }

        public override void Dispose()
        {
            _employeeRepo.Dispose();
        }

        [Fact]
        public void ShouldUpdateEmployee()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employeeID = 15;
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                Assert.Equal("Diane", employee.FirstName);
                Assert.Equal("Margheim", employee.LastName);
                Assert.Equal(EmailPromoPreference.NoPromotions, employee.EmailPromotion);

                employee.FirstName = "Briana";
                employee.LastName = "Prudhone";
                employee.EmailPromotion = EmailPromoPreference.AdventureWorksOnly;
                _employeeRepo.UpdateEmployee(employee);

                var edited = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                Assert.Equal("Briana", edited.FirstName);
                Assert.Equal("Prudhone", edited.LastName);
                Assert.Equal(EmailPromoPreference.AdventureWorksOnly, edited.EmailPromotion);
            }
        }

        [Fact]
        public void ShouldUpdateEmployePhoneNumber()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employeeID = 15;
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                var phone = employee.Phones
                    .Where(ph => ph.PhoneNumber == "815-555-0138")
                    .FirstOrDefault<PersonPhone>();

                Assert.NotNull(phone);

                employee.Phones.Add(new PersonPhone { PhoneNumber = "914-456-1234", PhoneNumberTypeID = 1 });
                employee.Phones.Remove(phone);

                _employeeRepo.UpdateEmployee(employee);
                var count = employee.Phones.Count;
                Assert.Equal(1, employee.Phones.Count);

                var phone2 = employee.Phones
                    .Where(ph => ph.PhoneNumber == "914-456-1234")
                    .FirstOrDefault<PersonPhone>();

                Assert.NotNull(phone2);
            }
        }

        [Fact]
        public void ShouldUpdateEmployeeEmailAddress()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employeeID = 15;
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                employee.EmailAddressObj.PersonEmailAddress = "diane.j.margheim@adventure-works.com";
                var result = _employeeRepo.UpdateEmployee(employee);

                employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                Assert.Equal("diane.j.margheim@adventure-works.com", employee.EmailAddressObj.PersonEmailAddress);
            }
        }

        [Fact]
        public void ShouldUpdateEmployeeFields()
        {
            var employeeID = 16;
            var beforeEdit = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
            Assert.Equal("998320692", beforeEdit.EmployeeObj.NationalIDNumber);
            Assert.Equal("adventure-works\\jossef0", beforeEdit.EmployeeObj.LoginID);
            Assert.Equal(new DateTime(1959, 3, 11), beforeEdit.EmployeeObj.BirthDate);

            beforeEdit.EmployeeObj.NationalIDNumber = "999999999";
            beforeEdit.EmployeeObj.LoginID = "adventure-works\\jossef99";
            beforeEdit.EmployeeObj.BirthDate = new DateTime(1965, 3, 11);
            _employeeRepo.UpdateEmployee(beforeEdit);

            var afterEdit = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
            Assert.Equal("999999999", afterEdit.EmployeeObj.NationalIDNumber);
            Assert.Equal("adventure-works\\jossef99", afterEdit.EmployeeObj.LoginID);
            Assert.Equal(new DateTime(1965, 3, 11), afterEdit.EmployeeObj.BirthDate);
        }

        [Fact]
        public void ShouldUpdateEmployeeAddress()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.PersonNavigation.FirstName == "Jossef" &&
                                                               e.EmployeeObj.PersonNavigation.LastName == "Goldberg");

                Assert.NotNull(employee);

                var employeeAddress = _employeeRepo.FindEmployeeAddress(employee.BusinessEntityID);

                Assert.NotNull(employeeAddress);
                Assert.Equal("5670 Bel Air Dr.", employeeAddress.AddressLine1);
                Assert.Equal("Renton", employeeAddress.City);
                Assert.Equal("98055", employeeAddress.PostalCode);

                employeeAddress.AddressLine1 = "12354 Big Wide Street";
                employeeAddress.City = "Rentonville";
                employeeAddress.PostalCode = "98055-8055";

                _employeeRepo.UpdateEmployeeAddress(employeeAddress);
                employeeAddress = _employeeRepo.FindEmployeeAddress(employee.BusinessEntityID);

                Assert.NotNull(employeeAddress);
                Assert.Equal("12354 Big Wide Street", employeeAddress.AddressLine1);
                Assert.Equal("Rentonville", employeeAddress.City);
                Assert.Equal("98055-8055", employeeAddress.PostalCode);
            }
        }

        [Fact]
        public void ShouldUpdateEmployeeDepartmentHistory()
        {
            // DepartmentHistory has 6 fields, the first 4 are part of the primary
            // and can not be edited. To edit the data requires replacing the record

            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employeeID = 16;
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);

                var deptHistory = employee.EmployeeObj.DepartmentHistories
                    .Where(hist => hist.DepartmentID == 10 && hist.ShiftID == 3 && hist.StartDate == new DateTime(2008, 1, 24))
                    .FirstOrDefault<EmployeeDepartmentHistory>();

                Assert.NotNull(deptHistory);

                var newDeptHistory = new EmployeeDepartmentHistory
                {
                    DepartmentID = deptHistory.DepartmentID,
                    ShiftID = deptHistory.ShiftID,
                    StartDate = new DateTime(2008, 2, 28)
                };

                employee.EmployeeObj.DepartmentHistories.Add(newDeptHistory);
                employee.EmployeeObj.DepartmentHistories.Remove(deptHistory);
                _employeeRepo.UpdateEmployee(employee);

                employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == employeeID);
                var afterEdit = employee.EmployeeObj.DepartmentHistories
                    .Where(hist => hist.DepartmentID == 10 && hist.ShiftID == 3 && hist.StartDate == new DateTime(2008, 2, 28))
                    .FirstOrDefault<EmployeeDepartmentHistory>();

                Assert.NotNull(afterEdit);
            }
        }
    }
}