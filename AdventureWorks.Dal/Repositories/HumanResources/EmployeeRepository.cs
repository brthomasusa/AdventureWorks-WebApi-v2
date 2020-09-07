using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class EmployeeRepository : RepositoryBase<EmployeeDomainObj>, IEmployeeRepository
    {
        private const string CLASSNAME = "EmployeeRepository";

        public EmployeeRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<EmployeeDomainObj> GetEmployees(EmployeeParameters employeeParameters)
        {
            Expression<Func<EmployeeDomainObj, bool>> filterCriteria;
            var employeeStatus = employeeParameters.EmployeeStatus.ToUpper();

            if (employeeStatus == "ACTIVE")
            {
                filterCriteria = ee => ee.IsActive == true;
            }
            else if (employeeStatus == "INACTIVE")
            {
                filterCriteria = ee => ee.IsActive == false;
            }
            else
            {
                filterCriteria = ee => ee.IsActive == true || ee.IsActive == false;
            }

            var employeeList = DbContext.EmployeeDomainObj.Where(filterCriteria).AsQueryable();

            SearchByName(ref employeeList, employeeParameters.FirstName, employeeParameters.LastName);

            return PagedList<EmployeeDomainObj>.ToPagedList(
                employeeList.OrderBy(ee => ee.LastName).ThenBy(ee => ee.FirstName).ThenBy(ee => ee.MiddleName),
                employeeParameters.PageNumber,
                employeeParameters.PageSize
            );
        }

        public EmployeeDomainObj GetEmployeeByID(int businessEntityID)
        {
            return DbContext.EmployeeDomainObj
                .Where(employee => employee.BusinessEntityID == businessEntityID)
                .AsQueryable()
                .FirstOrDefault();
        }

        public EmployeeDomainObj GetEmployeeByIDWithDetails(int businessEntityID)
        {
            var employee = DbContext.EmployeeDomainObj
                .Where(employee => employee.BusinessEntityID == businessEntityID)
                .AsQueryable()
                .FirstOrDefault();

            if (employee != null)
            {
                employee.DepartmentHistories.AddRange(
                    DbContext.EmployeeDepartmentHistory.Where(dh => dh.BusinessEntityID == businessEntityID).ToList()
                );

                employee.PayHistories.AddRange(
                    DbContext.EmployeePayHistory.Where(ph => ph.BusinessEntityID == businessEntityID).ToList()
                );

                employee.Addresses.AddRange(
                    DbContext.AddressDomainObj.Where(a => a.ParentEntityID == businessEntityID).ToList()
                );
            }

            return employee;
        }

        public void CreateEmployee(EmployeeDomainObj employeeDomainObj)
        {
            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var bizEntity = new BusinessEntity { };
                DbContext.BusinessEntity.Add(bizEntity);
                Save();

                employeeDomainObj.BusinessEntityID = bizEntity.BusinessEntityID;

                var person = new AdventureWorks.Models.Person.Person { };
                person.BusinessEntityID = bizEntity.BusinessEntityID;
                person.Map(employeeDomainObj);
                person.EmailAddressObj = new EmailAddress
                {
                    BusinessEntityID = bizEntity.BusinessEntityID,
                    PersonEmailAddress = employeeDomainObj.EmailAddress
                };
                person.PasswordObj = new PersonPWord
                {
                    BusinessEntityID = bizEntity.BusinessEntityID,
                    PasswordHash = employeeDomainObj.PasswordHash,
                    PasswordSalt = employeeDomainObj.PasswordSalt
                };

                var employee = new Employee { };
                employee.Map(employeeDomainObj);
                employee.BusinessEntityID = bizEntity.BusinessEntityID;

                person.EmployeeObj = employee;
                DbContext.Person.Add(person);
                Save();
            }
        }

        public void UpdateEmployee(EmployeeDomainObj employeeDomainObj)
        {
            var person = DbContext.Person
                .Where(p => p.BusinessEntityID == employeeDomainObj.BusinessEntityID)
                .Include(p => p.EmployeeObj)
                .Include(p => p.EmailAddressObj)
                .Include(p => p.PasswordObj)
                .FirstOrDefault();

            if (person == null)
            {
                string msg = $"Error: Update failed; unable to locate an employee in the database with ID '{employeeDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateEmployee " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            person.Map(employeeDomainObj);
            person.EmployeeObj.Map(employeeDomainObj);
            person.EmailAddressObj.PersonEmailAddress = employeeDomainObj.EmailAddress;
            person.PasswordObj.PasswordHash = employeeDomainObj.PasswordHash;
            person.PasswordObj.PasswordSalt = employeeDomainObj.PasswordSalt;

            DbContext.Person.Update(person);
            Save();
        }

        public void DeleteEmployee(EmployeeDomainObj employeeDomainObj)
        {
            var person = DbContext.Person
                .Where(p => p.BusinessEntityID == employeeDomainObj.BusinessEntityID)
                .Include(p => p.EmployeeObj)
                .FirstOrDefault();

            if (person == null)
            {
                string msg = $"Error: Update failed; unable to locate an employee in the database with ID '{employeeDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteEmployee " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            person.EmployeeObj.IsActive = false;

            DbContext.Person.Update(person);
            Save();
        }

        private void SearchByName(ref IQueryable<EmployeeDomainObj> employees, string firstName, string lastName)
        {
            if (!employees.Any() || (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName)))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                employees = employees.Where(e => e.FirstName.ToLower().Contains(firstName.Trim().ToLower()) &&
                                                e.LastName.ToLower().Contains(lastName.Trim().ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(firstName))
            {
                employees = employees.Where(e => e.FirstName.ToLower().Contains(firstName.Trim().ToLower()));
            }
            else
            {
                employees = employees.Where(e => e.LastName.ToLower().Contains(lastName.Trim().ToLower()));
            }
        }
    }
}