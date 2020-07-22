using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IEmployeeRepo : IRepo<Employee>
    {
        IEnumerable<PersonEmployee> GetAllPeopleEmployees();

        PersonEmployee FindPersonEmployee(int personID);

        IEnumerable<PersonClass> GetAllEmployees();

        PersonClass FindEmployee(Expression<Func<PersonClass, bool>> predicate);

        int AddEmployee(BusinessEntity employee, Address employeeAddress);

        int UpdateEmployee(PersonClass employee);

        int DeleteEmployee(PersonClass employee);

        Address FindEmployeeAddress(int BusinessEntityID);

        int AddEmployeeAddress(Address employeeAddress);

        int UpdateEmployeeAddress(Address employeeAddress);

        int DeleteEmployeeAddress(Address employeeAddress);

    }
}