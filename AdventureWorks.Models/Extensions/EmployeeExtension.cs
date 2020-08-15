using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Models.Extensions
{
    public static class EmployeeExtension
    {
        public static void Map(this Employee entityObj, EmployeeDomainObj domainObj)
        {
            entityObj.BusinessEntityID = domainObj.BusinessEntityID;
            entityObj.NationalIDNumber = domainObj.NationalIDNumber;
            entityObj.LoginID = domainObj.LoginID;
            entityObj.JobTitle = domainObj.JobTitle;
            entityObj.MaritalStatus = domainObj.MaritalStatus;
            entityObj.Gender = domainObj.Gender;
            entityObj.HireDate = domainObj.HireDate;
            entityObj.IsSalaried = domainObj.IsSalaried;
            entityObj.VacationHours = domainObj.VacationHours;
            entityObj.SickLeaveHours = domainObj.SickLeaveHours;
            entityObj.BirthDate = domainObj.BirthDate;
            entityObj.IsActive = domainObj.IsActive;
        }
    }
}