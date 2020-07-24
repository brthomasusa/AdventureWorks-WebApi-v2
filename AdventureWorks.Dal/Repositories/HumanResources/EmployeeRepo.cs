using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
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

        public IEnumerable<EmployeeViewModel> GetAllEmployeeViewModels() => Context.EmployeeViewModel.ToList();

        public EmployeeViewModel FindEmployeeViewModel(Expression<Func<EmployeeViewModel, bool>> predicate)
            => Context.EmployeeViewModel.Where(predicate).FirstOrDefault();

        public override int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var msg = "Error: The employee you are trying to delete does not exist. Try refreshing your screen.";

                throw new AdventureWorksConcurrencyExeception(msg, ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new AdventureWorksRetryLimitExceededException("There is a problem with your connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains("AK_Employee_Login", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate employee login!", ex);
                    }
                    else if (sqlException.Message.Contains("AK_Employee_NationalIDNumber", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: There is an existing employee with this National ID number!", ex);
                    }
                    else if (sqlException.Message.Contains("IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: There is an existing entity with this address!", ex);
                    }
                }

                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
        }

        // TODO If just populating a list, then the includes are not needed
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

        public int AddEmployee(BusinessEntity employee)
        {
            Context.BusinessEntity.Add(employee);
            SaveChanges();

            return employee.BusinessEntityID;
        }

        public int AddEmployee(BusinessEntity employee, Address employeeAddress)
        {
            Action dbTransaction = () =>
            {
                int employeeID = AddEmployee(employee);

                employeeAddress.BusinessEntityAddressObj.BusinessEntityID = employeeID;
                Context.Address.Add(employeeAddress);
                SaveChanges();
            };

            ExecuteInATransaction(dbTransaction);

            return employee.BusinessEntityID;
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