using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using AdventureWorks.Models.ViewModel;

namespace AdventureWorks.Dal.Tests.CrudTests.HumanResources
{
    [Collection("AdventureWorks.Dal")]
    public class RetrieveTests : TestBase
    {
        [Fact]
        public void ShouldRetrieveCompleteEmployeeObjectGraphByID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var eeID = 18;

            var employeeObj = ctx.Person
                .AsNoTracking()
                .Where(person => person.BusinessEntityID == eeID)
                .Include(person => person.EmployeeObj)
                    .ThenInclude(employee => employee.DepartmentHistories)
                .Include(person => person.EmployeeObj)
                    .ThenInclude(employee => employee.PayHistories)
                .Single<AdventureWorks.Models.Person.Person>();

            var payHistoryCount = employeeObj.EmployeeObj.PayHistories.Count;
            var deptHistoryCount = employeeObj.EmployeeObj.DepartmentHistories.Count;

            Assert.NotNull(employeeObj);
            Assert.Equal(2, payHistoryCount);
            Assert.Equal(2, deptHistoryCount);
            Assert.Equal("245797967", employeeObj.EmployeeObj.NationalIDNumber);
        }

        [Fact]
        public void ShouldRetrieveCompleteEmployeeObjectGraphByNationalID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var employeeObj = ctx.Person
                .AsNoTracking()
                .Where(person => person.PersonType == "EM" && person.EmployeeObj.NationalIDNumber == "811994146")
                .Single<AdventureWorks.Models.Person.Person>();

            Assert.NotNull(employeeObj);
        }

        [Fact]
        public void ShouldRetrieveCompleteEmployeeObjectGraphByLoginID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var employeeObj = ctx.Person
                .AsNoTracking()
                .Where(person => person.PersonType == "EM" && person.EmployeeObj.LoginID == "adventure-works\\freddyk")
                .Single<AdventureWorks.Models.Person.Person>();

            Assert.NotNull(employeeObj);
        }

        [Fact]
        public void ShouldRetrieveOneEmployeeByFirstAndLastName()
        {
            SampleDataInitialization.InitializeData(ctx);

            var employee = ctx.Employee
                .Where(e => e.PersonNavigation.FirstName == "Terri" && e.PersonNavigation.LastName == "Duffy")
                .First();

            Assert.Equal(18, employee.BusinessEntityID);
        }

        [Fact]
        public void ShouldRetrieveOneEmployeeByBusinessEntityID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var bizEntityID = 18;
            var employee = ctx.Employee.Find(bizEntityID);


            Assert.Equal("Duffy", employee.PersonNavigation.LastName);
        }

        [Fact]
        public void ShouldRetrieveAllEmployeeViewModels()
        {
            SampleDataInitialization.InitializeData(ctx);

            var employees = ctx.EmployeeViewModel.ToList();

            Assert.NotNull(employees);
            var count = employees.Count;
            Assert.Equal(6, count);
        }

        [Fact]
        public void ShouldRetrieveOneEmployeeViewModelByID()
        {
            SampleDataInitialization.InitializeData(ctx);

            var eeID = 18;
            var employee = ctx.EmployeeViewModel
                .Where(e => e.BusinessEntityID == eeID)
                .FirstOrDefault();

            Assert.NotNull(employee);
            Assert.Equal("Terri", employee.FirstName);
            Assert.Equal("Duffy", employee.LastName);
            Assert.Equal(1, employee.VacationHours);
        }

        [Fact]
        public void ShouldRetrieveOneDepartmentByID()
        {
            SampleDataInitialization.InitializeData(ctx);

            short deptID = 15;
            var dept = ctx.Department.Find(deptID);

            Assert.NotNull(dept);
            Assert.Equal("Engineering", dept.Name);
        }

        [Fact]
        public void ShouldRetrieveOneDepartmentByName()
        {
            SampleDataInitialization.InitializeData(ctx);

            var deptName = "Engineering";
            var dept = ctx.Department.Where(d => d.Name == deptName).First();

            Assert.Equal(15, dept.DepartmentID);
            Assert.Equal("Research and Development", dept.GroupName);
        }
    }
}