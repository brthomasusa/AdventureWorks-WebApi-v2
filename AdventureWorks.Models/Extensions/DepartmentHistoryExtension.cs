using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Models.Extensions
{
    public static class DepartmentHistoryExtension
    {
        public static void Map(this EmployeeDepartmentHistory destination, EmployeeDepartmentHistory source)
        {
            destination.BusinessEntityID = source.BusinessEntityID;
            destination.DepartmentID = source.DepartmentID;
            destination.ShiftID = source.ShiftID;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
        }
    }
}