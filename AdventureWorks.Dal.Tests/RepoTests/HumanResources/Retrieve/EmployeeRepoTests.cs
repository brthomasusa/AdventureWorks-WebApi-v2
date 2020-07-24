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

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Retrieve
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
        public void ShouldGetAllEmployees()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employees = _employeeRepo.GetAllEmployees();

                var count = employees.Count();
                Assert.Equal(6, count);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeByID()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("adventure-works\\ken0", employee.EmployeeObj.LoginID);
                Assert.Equal("295847284", employee.EmployeeObj.NationalIDNumber);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeByFirstAndLastName()
        {
            var employee = _employeeRepo.FindEmployee(e => e.FirstName == "Ken" && e.LastName == "Sanchez");
            Assert.NotNull(employee);
        }

        [Fact]
        public void ShouldGetOneEmployeeByEmployeeLoginID()
        {
            var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.LoginID == "adventure-works\\ken0");
            Assert.NotNull(employee);
        }

        [Fact]
        public void ShouldGetOneEmployeeWithDeptHistory()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                var count = employee.EmployeeObj.DepartmentHistories.Count();
                Assert.Equal(1, count);
                Assert.Equal(10, employee.EmployeeObj.DepartmentHistories[0].DepartmentID);
                Assert.Equal(3, employee.EmployeeObj.DepartmentHistories[0].ShiftID);
                Assert.Equal(new DateTime(2009, 1, 14), employee.EmployeeObj.DepartmentHistories[0].StartDate);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithPayHistory()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                var count = employee.EmployeeObj.PayHistories.Count();
                Assert.Equal(1, count);
                Assert.Equal(new DateTime(2009, 1, 14), employee.EmployeeObj.PayHistories[0].RateChangeDate);
                Assert.Equal(125.50M, employee.EmployeeObj.PayHistories[0].Rate);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithRelatedPhoneNumber()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("697-555-0142", employee.Phones[0].PhoneNumber);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithRelatedEmailAddress()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("ken@adventure-works.com", employee.EmailAddressObj.PersonEmailAddress);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithRelatedEmailPassword()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=", employee.PasswordObj.PasswordHash);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithRelatedPhoneEmailAndPword()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployee(e => e.EmployeeObj.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("697-555-0142", employee.Phones[0].PhoneNumber);
                Assert.Equal("ken@adventure-works.com", employee.EmailAddressObj.PersonEmailAddress);
                Assert.Equal("pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=", employee.PasswordObj.PasswordHash);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeWithRelatedAddress()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var bizEntityAddress = _employeeRepo.FindEmployeeAddress(1);

                Assert.NotNull(bizEntityAddress);
                Assert.Equal("4350 Minute Dr.", bizEntityAddress.AddressLine1);
                Assert.Equal("Newport Hills", bizEntityAddress.City);
                Assert.Equal("98006", bizEntityAddress.PostalCode);
            }
        }

        [Fact]
        public void ShouldGetAllEmployeeViewModels()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employees = _employeeRepo.GetAllEmployeeViewModels();

                Assert.NotNull(employees);
                int count = employees.Count();
                Assert.Equal(6, count);
            }
        }

        [Fact]
        public void ShouldGetOneEmployeeViewModelByID()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var employee = _employeeRepo.FindEmployeeViewModel(e => e.BusinessEntityID == 1);

                Assert.NotNull(employee);
                Assert.Equal("Ken", employee.FirstName);
            }
        }
    }
}