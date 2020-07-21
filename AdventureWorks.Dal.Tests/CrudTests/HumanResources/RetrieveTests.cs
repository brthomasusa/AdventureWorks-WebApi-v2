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

            Assert.NotNull(employeeObj);
            Assert.Equal("245797967", employeeObj.EmployeeObj.NationalIDNumber);
            Assert.True(employeeObj.EmployeeObj.PayHistories.Count() == 1);
            Assert.True(employeeObj.EmployeeObj.DepartmentHistories.Count() == 1);
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
        public void ShouldRetrieveOneEmployeeFromView()
        {
            SampleDataInitialization.InitializeData(ctx);

            var employee = ctx.PersonEmployee
                .Where(vc => vc.BusinessEntityID == 1)
                .First<PersonEmployee>();

            Assert.Equal("ken@adventure-works.com", employee.EmailAddress);
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
    }
}