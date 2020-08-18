using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Models.Extensions
{
    public static class PayHistoryExtension
    {
        public static void Map(this EmployeePayHistory destination, EmployeePayHistory source)
        {
            destination.BusinessEntityID = source.BusinessEntityID;
            destination.RateChangeDate = source.RateChangeDate;
            destination.Rate = source.Rate;
            destination.PayFrequency = source.PayFrequency;
        }
    }
}