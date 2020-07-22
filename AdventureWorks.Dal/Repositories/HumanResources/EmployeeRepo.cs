using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.ViewModel;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class EmployeeRepo : RepoBase<Employee>, IEmployeeRepo
    {
        const int HOME_ADDRESS = 2;

        public EmployeeRepo(AdventureWorksContext context) : base(context) { }

        public EmployeeRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }

        public IEnumerable<PersonEmployee> GetAllPeopleEmployees() => Context.PersonEmployee.ToList();

        public IEnumerable<PersonClass> GetAllEmployees()
            => Context.Person
                .Where(p => p.PersonType == "EM")
                .Include(ee => ee.Phones)
                .Include(ee => ee.EmailAddressObj)
                .Include(ee => ee.PasswordObj)
                .Include(ee => ee.EmployeeObj)
                .ToList();

        public PersonClass FindEmployee(Expression<Func<PersonClass, bool>> predicate)
            => Context.Person
                .Where(person => person.PersonType == "EM")
                .Where(predicate)
                .Include(ee => ee.Phones)
                .Include(ee => ee.EmailAddressObj)
                .Include(ee => ee.PasswordObj)
                .Include(ee => ee.EmployeeObj)
                .Include(ee => ee.EmployeeObj).ThenInclude(ee => ee.DepartmentHistories)
                .Include(ee => ee.EmployeeObj).ThenInclude(ee => ee.PayHistories)
                .FirstOrDefault<PersonClass>();

        public PersonEmployee FindPersonEmployee(int personID)
            => Context.PersonEmployee
                .FirstOrDefault(ee => ee.BusinessEntityID == personID);

        public int AddEmployee(BusinessEntity employee, Address employeeAddress)
        {
            Context.BusinessEntity.Add(employee);
            SaveChanges();

            int businessEntityID = employee.BusinessEntityID;

            employeeAddress.BusinessEntityAddressObj.BusinessEntityID = businessEntityID;
            Context.Address.Add(employeeAddress);
            SaveChanges();

            return businessEntityID;
        }

        public int UpdateEmployee(PersonClass employee)
        {
            Context.Person.Update(employee);
            SaveChanges();
            return employee.BusinessEntityID;
        }

        public int DeleteEmployee(PersonClass employee)
        {
            employee.EmployeeObj.IsActive = false;
            Context.Employee.Update(employee.EmployeeObj);
            return SaveChanges();
        }

        public Address FindEmployeeAddress(int businessEntityID)
        {
            var bizEntityAddress = Context.BusinessEntityAddress
                .Where(bea => bea.BusinessEntityID == businessEntityID && bea.AddressTypeID == HOME_ADDRESS)
                .Include(a => a.AddressNavigation)
                .FirstOrDefault<BusinessEntityAddress>();

            return (bizEntityAddress == null ? null : bizEntityAddress.AddressNavigation);
        }

        public int AddEmployeeAddress(Address employeeAddress)
        {
            Context.Address.Update(employeeAddress);
            SaveChanges();
            return employeeAddress.AddressID;
        }

        public int UpdateEmployeeAddress(Address employeeAddress)
        {
            Context.Address.Update(employeeAddress);
            return SaveChanges();
        }

        public int DeleteEmployeeAddress(Address employeeAddress)
        {
            Context.Address.Remove(employeeAddress);
            return SaveChanges();
        }
    }
}