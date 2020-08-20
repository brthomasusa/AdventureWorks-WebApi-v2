using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Models.Extensions
{
    public static class DepartmentExtension
    {
        public static void Map(this Department destination, Department source)
        {
            destination.DepartmentID = source.DepartmentID;
            destination.Name = source.Name;
            destination.GroupName = source.GroupName;
        }
    }
}