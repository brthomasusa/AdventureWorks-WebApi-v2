using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdventureWorks.Dal.Initialization;
using AdventureWorks.Dal.Tests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.Tests.CrudTests.HumanResources
{
    [Collection("AdventureWorks.Dal")]
    public class CreationTests : TestBase
    {
        [Fact]
        public void ShouldAddNewEmployeeRecord()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "222222222",
                            LoginID = "adventure-works\\jdoe0",
                            JobTitle = "Software Engineer",
                            BirthDate = new DateTime(1991, 1, 29),
                            MaritalStatus = "S",
                            Gender = "F",
                            HireDate = new DateTime(2020, 6, 6),
                            VacationHours = 0,
                            SickLeaveHours = 0
                        }
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);

                var employee = ctx.Employee.Find(person.BusinessEntityID);
                Assert.NotNull(employee);
                Assert.Equal(businessEntity.PersonObj.BusinessEntityID, person.BusinessEntityID);
                Assert.Equal("222222222", employee.NationalIDNumber);

            }
        }

        [Fact]
        public void ShouldAddNewEmployeeWithDepartmentHistory()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "222222222",
                            LoginID = "adventure-works\\jdoe0",
                            JobTitle = "Software Engineer",
                            BirthDate = new DateTime(1991, 1, 29),
                            MaritalStatus = "S",
                            Gender = "F",
                            HireDate = new DateTime(2020, 6, 6),
                            VacationHours = 0,
                            SickLeaveHours = 0,
                            DepartmentHistories = {
                                new EmployeeDepartmentHistory
                                {
                                    DepartmentID = 5,
                                    ShiftID = 1,
                                    StartDate = new DateTime(2020, 6, 6)
                                }
                            }
                        }
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);

                var employee = ctx.Employee.Find(person.BusinessEntityID);
                Assert.NotNull(employee);
                Assert.Equal(5, employee.DepartmentHistories[0].DepartmentID);
                Assert.Equal(new DateTime(2020, 6, 6), employee.DepartmentHistories[0].StartDate);

            }
        }

        [Fact]
        public void ShouldAddNewEmployeeWithPayHistory()
        {
            SampleDataInitialization.InitializeData(ctx);
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var businessEntity = new BusinessEntity
                {
                    PersonObj = new AdventureWorks.Models.Person.Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Jane",
                        MiddleName = "J",
                        LastName = "Doe",
                        Suffix = "Ms.",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "222222222",
                            LoginID = "adventure-works\\jdoe0",
                            JobTitle = "Software Engineer",
                            BirthDate = new DateTime(1991, 1, 29),
                            MaritalStatus = "S",
                            Gender = "F",
                            HireDate = new DateTime(2020, 6, 6),
                            VacationHours = 0,
                            SickLeaveHours = 0,
                            PayHistories = {
                                new EmployeePayHistory
                                {
                                    RateChangeDate = new DateTime(2020, 6, 6),
                                    Rate = 85.00M,
                                    PayFrequency = PayFrequency.Biweekly
                                }
                            }
                        }
                    }
                };

                ctx.BusinessEntity.Add(businessEntity);
                ctx.SaveChanges();

                var person = ctx.Person
                    .AsNoTracking()
                    .Where(p => p.FirstName == "Jane" && p.MiddleName == "J" && p.LastName == "Doe")
                    .SingleOrDefault<AdventureWorks.Models.Person.Person>();

                Assert.NotNull(person);

                var employee = ctx.Employee.Find(person.BusinessEntityID);
                Assert.NotNull(employee);
                Assert.Equal(new DateTime(2020, 6, 6), employee.PayHistories[0].RateChangeDate);

            }
        }
    }
}