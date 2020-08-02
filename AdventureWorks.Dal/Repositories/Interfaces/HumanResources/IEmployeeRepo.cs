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
        IEnumerable<EmployeeViewModel> GetAllEmployeeViewModels();

        EmployeeViewModel FindEmployeeViewModel(Expression<Func<EmployeeViewModel, bool>> predicate);

        IEnumerable<PersonClass> GetAllEmployees();

        PersonClass FindEmployee(Expression<Func<PersonClass, bool>> predicate);

        int AddEmployee(BusinessEntity employee);

        int AddEmployee(BusinessEntity employee, Address employeeAddress);

        int UpdateEmployee(PersonClass employee);

        int DeleteEmployee(PersonClass employee);

        Address FindEmployeeAddress(int BusinessEntityID);

        int AddEmployeeAddress(Address employeeAddress);

        int UpdateEmployeeAddress(Address employeeAddress);

        int DeleteEmployeeAddress(Address employeeAddress);

    }
}